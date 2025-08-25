using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPawDiaryApp.Models
{
    public class OneTimeActivity
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime DateAndTime { get; set; }
        public bool isCompleted { get; set; } 


    }
}