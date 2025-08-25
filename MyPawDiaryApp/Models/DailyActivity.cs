using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyPawDiaryApp.Models
{
    public class DailyActivity
    {
        public int Id { get; set; }

        public int PetId { get; set; }
        public Pet Pet { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public string Name { get; set; }
        public string Notes { get; set; }

        public virtual List<DailyActivityCompletion> Completions { get; set; }

        [NotMapped]
        public bool IsDoneToday { get; set; }

    }
}