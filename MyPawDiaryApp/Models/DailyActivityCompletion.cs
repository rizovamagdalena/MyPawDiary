using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MyPawDiaryApp.Models
{
    public class DailyActivityCompletion
    {
        public int Id { get; set; }

        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }


        public int ActivityId { get; set; }
        public virtual DailyActivity Activity { get; set; }

        public DateTime Date { get; set; }

    }
}