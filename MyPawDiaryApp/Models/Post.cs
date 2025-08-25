using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPawDiaryApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string PicturePath { get; set; } 
        public string Caption { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<PostLike> Likes { get; set; }
    }

}