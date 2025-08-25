using Microsoft.AspNet.Identity;
using MyPawDiaryApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyPawDiaryApp.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pets
        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();
            var pets = db.Pets
                .Include(p => p.Owner)
                .Where(p => p.OwnerId == userId);

            return View(pets.ToList());
        }

        // GET: Pets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pet = db.Pets
            .Include(p => p.DailyActivities.Select(a => a.Completions))
            .Include(p => p.OneTimeActivities)
            .Include(p => p.MedicalRecords)
            .FirstOrDefault(p => p.Id == id);

            var currentUserId = User.Identity.GetUserId();

            if (pet.OwnerId != currentUserId)
            {
                return new HttpUnauthorizedResult("Notfound.");
            }

            foreach (var activity in pet.DailyActivities)
            {
                activity.IsDoneToday = activity.Completions
                                          .Any(c => c.Date.Date == DateTime.Today);
            }

            return View(pet);
        }

        // GET: Pets/Create
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Species,Breed,DateOfBirth,Gender,OwnerId")] Pet pet, HttpPostedFileBase PetPhoto)
        {
            if (ModelState.IsValid)
            {
                if (PetPhoto != null && PetPhoto.ContentLength > 0)
                {
                    var uploadDir = Server.MapPath("~/Content/images/pet_pictures");

                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    var fileName = Guid.NewGuid() + Path.GetExtension(PetPhoto.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    PetPhoto.SaveAs(filePath);

                    pet.Photo = "/Content/images/pet_pictures/" + fileName;
                }

                db.Pets.Add(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", pet.OwnerId);
            return View(pet);
        }


        // GET: Pets/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);

            var currentUserId = User.Identity.GetUserId();

            if (pet.OwnerId != currentUserId)
            {
                return new HttpUnauthorizedResult("Notfound.");
            }

            if (pet == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", pet.OwnerId);
            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HttpPostedFileBase PetPhoto, FormCollection form)
        {
            var pet = db.Pets.Find(id);
            if (pet == null)
            {
                System.Diagnostics.Debug.WriteLine("Pet not found with id: " + id);
                return HttpNotFound();
            }

            System.Diagnostics.Debug.WriteLine("Editing pet: " + pet.Name);

            if (TryUpdateModel(pet, "", new string[] { "Name", "Species", "Breed", "DateOfBirth", "Gender" }))
            {
                System.Diagnostics.Debug.WriteLine("Fields updated successfully.");

                if (PetPhoto != null)
                {
                    System.Diagnostics.Debug.WriteLine("PetPhoto is not null. FileName: " + PetPhoto.FileName);
                    System.Diagnostics.Debug.WriteLine("ContentLength: " + PetPhoto.ContentLength);

                    if (PetPhoto.ContentLength > 0)
                    {
                        var uploadDir = Server.MapPath("~/Content/images/pet_pictures");
                        System.Diagnostics.Debug.WriteLine("Upload directory: " + uploadDir);

                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                            System.Diagnostics.Debug.WriteLine("Created upload directory.");
                        }

                        var fileName = Guid.NewGuid() + Path.GetExtension(PetPhoto.FileName);
                        var filePath = Path.Combine(uploadDir, fileName);

                        System.Diagnostics.Debug.WriteLine("Saving file to: " + filePath);

                        PetPhoto.SaveAs(filePath);
                        pet.Photo = "/Content/images/pet_pictures/" + fileName;

                        System.Diagnostics.Debug.WriteLine("Photo saved successfully. Pet.Photo path: " + pet.Photo);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No photo uploaded.");
                }

                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine("Pet saved to database.");
                return RedirectToAction("Index");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("TryUpdateModel failed.");
            }

            return View(pet);
        }


        // GET: Pets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
