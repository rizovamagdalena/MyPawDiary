using MyPawDiaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyPawDiaryApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //SeedActivityTypes();

        }

        //private void SeedActivityTypes()
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        if (!db.ActivityTypes.Any())
        //        {
        //            db.ActivityTypes.AddRange(new[]
        //            {
        //         new ActivityType { Name = "Breakfast", Icon = "coffee", IsRecurringDaily = true },
        //        new ActivityType { Name = "Dinner", Icon = "utensils", IsRecurringDaily = true },
        //        new ActivityType { Name = "Fresh Water", Icon = "tint", IsRecurringDaily = true },
        //        new ActivityType { Name = "Walk", Icon = "walking", IsRecurringDaily = true },
        //        new ActivityType { Name = "Vet Visit", Icon = "stethoscope", IsRecurringDaily = false },
        //        new ActivityType { Name = "Grooming", Icon = "cut", IsRecurringDaily = false },
        //        new ActivityType { Name = "Birthday", Icon = "birthday-cake", IsRecurringDaily = false },
        //        new ActivityType { Name = "Something Else", Icon = "question", IsRecurringDaily = false}
        //            });

        //            db.SaveChanges();
        //        }
        //    }
        //}
    }
}
