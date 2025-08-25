using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPawDiaryApp.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public int ActivityId { get; set; }
        public virtual OneTimeActivity Activity { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

    }
}