using Microsoft.AspNet.Identity;
using MyPawDiaryApp.Models;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyPawDiaryApp.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities/CreateDailyActivity
        public ActionResult CreateDailyActivity(int? petId)
        {
            if (petId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pet = db.Pets.Find(petId);
            if (pet == null)
            {
                return HttpNotFound();
            }

            ViewBag.PetId = pet.Id;
            ViewBag.PetName = pet.Name;

            return View();
        }

        // POST: Activities/CreateDailyActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDailyActivity([Bind(Include = "PetId,Name,Notes")] DailyActivity dailyActivity)
        {
            if (ModelState.IsValid)
            {
                dailyActivity.UserId = User.Identity.GetUserId();
                db.DailyActivities.Add(dailyActivity);
                db.SaveChanges();
                return RedirectToAction("Details", "Pets", new { id = dailyActivity.PetId });
            }

            var pet = db.Pets.Find(dailyActivity.PetId);
            ViewBag.PetId = pet?.Id;
            ViewBag.PetName = pet?.Name;

            return RedirectToAction("Details", "Pets", new { id = dailyActivity.PetId });
        }

        // GET: Activities/CreateOneTimeActivity
        public ActionResult CreateOneTimeActivity(int? petId)
        {
            if (petId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pet = db.Pets.Find(petId);
            if (pet == null)
            {
                return HttpNotFound();
            }

            ViewBag.PetId = pet.Id;
            ViewBag.PetName = pet.Name;

            return View();
        }

        // POST: Activities/CreateOneTimeActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOneTimeActivity([Bind(Include = "PetId,Name,Notes,DateAndTime")] OneTimeActivity oneTimeActivity)
        {
            if (ModelState.IsValid)
            {
                oneTimeActivity.UserId = User.Identity.GetUserId();
                oneTimeActivity.isCompleted = false; 
                db.OneTimeActivities.Add(oneTimeActivity);
                db.SaveChanges();
                return RedirectToAction("Details", "Pets", new { id = oneTimeActivity.PetId });
            }

            var pet = db.Pets.Find(oneTimeActivity.PetId);
            ViewBag.PetId = pet?.Id;
            ViewBag.PetName = pet?.Name;

            return RedirectToAction("Details", "Pets", new { id = oneTimeActivity.PetId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDailyActivity(int activityId, int? petId)
        {
            Debug.WriteLine("DeleteDailyActivity called. id = " + activityId + ", petId = " + petId);
            var allActivities = db.DailyActivities.ToList();
            Debug.WriteLine("All DailyActivities: " + string.Join(", ", allActivities.Select(a => a.Id)));


            var activity = db.DailyActivities 
                             .FirstOrDefault(a => a.Id == activityId);


            Debug.WriteLine("Activity. id = " + activity.Id + ", petId = " + petId);

            if (activity != null)
            {
                if (activity.Completions != null && activity.Completions.Any())
                {
                    db.DailyActivityCompletions.RemoveRange(activity.Completions);
                }

                db.DailyActivities.Remove(activity);
                db.SaveChanges();

                return RedirectToAction("Details", "Pets", new { id = activity.PetId });
            }

            if (petId.HasValue)
                return RedirectToAction("Details", "Pets", new { id = petId.Value });

            return RedirectToAction("Index", "Pets");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOneTimeActivity(int activityId)
        {
            var activity = db.OneTimeActivities.Find(activityId);

            if (activity == null)
            {
                return RedirectToAction("Index", "Pets");
            }

            var petId = activity.PetId;

            db.OneTimeActivities.Remove(activity);
            db.SaveChanges();

            return RedirectToAction("Details", "Pets", new { id = petId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMedicalRecord(int PetId, int ActivityId, string Notes)
        {
            var activity = db.OneTimeActivities.Find(ActivityId);
            if (activity == null)
            {
                return HttpNotFound("Selected activity not found.");
            }

            var record = new MedicalRecord
            {
                PetId = PetId,
                ActivityId = ActivityId,
                Date = activity.DateAndTime,
                Notes = Notes
            };

            db.MedicalRecords.Add(record);
            db.SaveChanges();

            return RedirectToAction("Details", "Pets", new { id = PetId });
        }


    }
}
