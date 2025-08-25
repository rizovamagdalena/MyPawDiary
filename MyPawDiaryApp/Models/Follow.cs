using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPawDiaryApp.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public string FollowerId { get; set; }
        public virtual ApplicationUser Follower { get; set; }
        public string FollowingId { get; set; }
        public virtual ApplicationUser Following { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}