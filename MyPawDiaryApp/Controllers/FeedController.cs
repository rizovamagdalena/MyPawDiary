using Microsoft.AspNet.Identity;
using MyPawDiaryApp.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MyPawDiaryApp.Controllers
{
    [Authorize]
    public class FeedController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Feed
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var pets = db.Pets
                .Where(p => p.OwnerId == userId)
                .Include(p => p.DailyActivities.Select(d => d.Completions))
                .Include(p => p.OneTimeActivities)
                .ToList();

            foreach (var pet in pets)
            {
                foreach (var daily in pet.DailyActivities)
                {
                    daily.IsDoneToday = daily.Completions
                        .Any(c => c.Date.Date == today);
                }
            }

            var recurringActivities = pets
                .SelectMany(p => p.DailyActivities)
                .ToList();

            var todayOneTimeActivities = pets
                .SelectMany(p => p.OneTimeActivities)
                .Where(a => a.DateAndTime.Date == today)
                .ToList();

            var tomorrowOneTimeActivities = pets
                .SelectMany(p => p.OneTimeActivities)
                .Where(a => a.DateAndTime.Date == tomorrow)
                .ToList();

            var friendsPostsEntities = db.Posts
                .Where(p => db.Follows
                              .Any(f => f.FollowerId == userId && f.FollowingId == p.UserId)
                            && p.CreatedAt >= today && p.CreatedAt < tomorrow)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .ToList();

            var friendsPosts = friendsPostsEntities.Select(p => new PostViewModel
            {
                Id = p.Id,
                Caption = p.Caption,
                PicturePath = p.PicturePath,
                User = p.User,
                LikesCount = p.Likes.Count,
                IsLikedByCurrentUser = p.Likes.Any(l => l.UserId == userId),
                CreatedAt = p.CreatedAt
            }).ToList();

            //System.Diagnostics.Debug.WriteLine("FriendsPosts count: " + friendsPosts.Count);
            //foreach (var post in friendsPosts)
            //{
            //    System.Diagnostics.Debug.WriteLine($"[FRIEND] PostId: {post.Id}, User: {post.User.FirstName}, Content: {post.Caption}, CreatedAt: {post.CreatedAt}");
            //}

       
            var model = new FeedViewModel
            {
                Pets = pets,
                Today = today,
                DailyActivities = recurringActivities,
                TodayOneTimeActivities = todayOneTimeActivities,
                TomorrowOneTimeActivities = tomorrowOneTimeActivities,
                FriendsPosts = friendsPosts
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkDone(int petId, int activityId, string activityType)
        {
            try
            {
                var today = DateTime.Today;
                var userId = User.Identity.GetUserId();

                if (activityType == "Daily")
                {
                    var completion = db.DailyActivityCompletions
                        .FirstOrDefault(c => c.PetId == petId
                                             && c.ActivityId == activityId
                                             && DbFunctions.TruncateTime(c.Date) == today);

                    if (completion != null)
                    {
                        db.DailyActivityCompletions.Remove(completion);
                    }
                    else
                    {
                        db.DailyActivityCompletions.Add(new DailyActivityCompletion
                        {
                            PetId = petId,
                            ActivityId = activityId,
                            Date = DateTime.Now
                        });
                    }
                }
                else if (activityType == "OneTime")
                {
                    var activity = db.OneTimeActivities
                        .FirstOrDefault(a => a.PetId == petId && a.Id == activityId);

                    if (activity != null)
                    {
                        activity.isCompleted = !activity.isCompleted;
                        db.Entry(activity).State = EntityState.Modified;
                    }
                    else
                    {
                        throw new Exception($"One-time activity not found. PetId={petId}, ActivityId={activityId}");
                    }
                }
                else
                {
                    throw new Exception($"Unknown activityType: {activityType}");
                }

                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MarkDone error: " + ex.ToString());

                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(string content, HttpPostedFileBase Photo)
        {
            var userId = User.Identity.GetUserId();

            var post = new Post
            {
                UserId = userId,
                Caption = content,
                CreatedAt = DateTime.Now
            };

            if (Photo != null && Photo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Photo.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images/posts"), fileName);
                Photo.SaveAs(path);
                post.PicturePath = "/Content/images/posts/" + fileName;
            }

            db.Posts.Add(post);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleLike(int postId)
        {
            var userId = User.Identity.GetUserId(); 
            var post = db.Posts.Include(p => p.Likes)
                               .FirstOrDefault(p => p.Id == postId);
            if (post == null)
                return Json(new { success = false });

            var existingLike = post.Likes.FirstOrDefault(l => l.UserId == userId);
            bool isLiked;

            if (existingLike != null)
            {
                db.Likes.Remove(existingLike);
                isLiked = false;
            }
            else
            {
                db.Likes.Add(new PostLike
                { 
                    PostId = postId,
                    UserId = userId
                });
                
                isLiked = true;
            }

            db.SaveChanges();

            return Json(new
            {
                success = true,
                likesCount = post.Likes.Count,
                isLiked = isLiked
            });
        }




    }
}
