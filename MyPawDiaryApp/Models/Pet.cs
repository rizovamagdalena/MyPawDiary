using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPawDiaryApp.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }

        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<DailyActivity> DailyActivities { get; set; }
        public virtual ICollection<OneTimeActivity> OneTimeActivities { get; set; }
        public virtual ICollection<DailyActivityCompletion> DailyActivityCompletions { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }


    }
}