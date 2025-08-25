using Microsoft.AspNet.Identity;
using MyPawDiaryApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPawDiaryApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JsonResult SearchUsers(string query)
        {
            var users = db.Users
                          .Where(u => u.FirstName.Contains(query) ||
                                      u.LastName.Contains(query) ||
                                      u.UserName.Contains(query))
                          .Select(u => new { u.FirstName, u.LastName, u.UserName, u.ProfilePhotoPath })
                          .Take(5)
                          .ToList();

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        // GET: /Profile/Posts
        public ActionResult Posts(string username)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) return HttpNotFound();

            var posts = db.Posts
                          .Where(p => p.UserId == user.Id)
                          .OrderByDescending(p => p.CreatedAt)
                          .ToList();

            return View(posts);
        }

        // GET: /Profile/Details
        [Authorize]
        public ActionResult Details(string username)
        {
            Debug.WriteLine("Details was called with username = " + username);

            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users
                                .Include("Pets")
                                .Include("Posts") 
                                .FirstOrDefault(u => u.Id == currentUserId);

            if (string.IsNullOrEmpty(username))
            {
                //Debug.WriteLine("in emptyusername, wich is for self profile");
                //Debug.WriteLine("user:" + currentUser.UserName);

                ViewBag.isCurrentUser = true;
                //Debug.WriteLine("curr user name = " + currentUser.UserName);
                //Debug.WriteLine("the usernamee = " + username);

                if (currentUser == null) return HttpNotFound();
                return View(currentUser);
            }
            var user = db.Users
                         .Include("Pets")
                         .Include("Posts")
                         .FirstOrDefault(u => u.UserName == username);

            if (user == null) return HttpNotFound();


            ViewBag.IsFollowing = db.Follows.Any(f => f.FollowerId == currentUserId && f.FollowingId == user.Id);
            ViewBag.isCurrentUser = currentUser.UserName == username;
            //Debug.WriteLine("curr user name = " + currentUser.UserName);
            //Debug.WriteLine("the usernamee = " + username);


            return View(user);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Follow(string userId)
        {
            var currentUserId = User.Identity.GetUserId();

            var alreadyFollowing = db.Follows.Any(f => f.FollowerId == currentUserId && f.FollowingId == userId);
            if (!alreadyFollowing)
            {
                db.Follows.Add(new Follow
                {
                    FollowerId = currentUserId,
                    FollowingId = userId
                });
                db.SaveChanges();
            }



            return RedirectToAction("Details", new { username = db.Users.Find(userId).UserName });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unfollow(string userId)
        {
            var currentUserId = User.Identity.GetUserId();
            var follow = db.Follows.FirstOrDefault(f => f.FollowerId == currentUserId && f.FollowingId == userId);
            if (follow != null)
            {
                db.Follows.Remove(follow);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { username = db.Users.Find(userId).UserName });
        }


        // GET: /Profile/Friends
        [Authorize]
        public ActionResult Friends()
        {
            // Get the logged-in user
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user == null) return HttpNotFound();

            // Get users this user follows
            var friends = db.Follows
                            .Where(f => f.FollowerId == user.Id)
                            .Select(f => f.Following)
                            .ToList();

            return View(friends);
        }

        public ActionResult EditProfile()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: /Profile/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ApplicationUser model, HttpPostedFileBase ProfilePhoto)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user == null)
                return HttpNotFound();

            user.FirstName = model.FirstName;
            user.Email = model.Email;

            if (ProfilePhoto != null && ProfilePhoto.ContentLength > 0)
            {

                var fileName = Path.GetFileName(ProfilePhoto.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images/profile_pictures"), fileName);
                ProfilePhoto.SaveAs(path);
                user.ProfilePhotoPath = "/Content/images/profile_pictures/" + fileName;
            }

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Profile", new { id = user.Id });
        }

    }
}