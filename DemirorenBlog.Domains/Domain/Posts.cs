using System;
using System.ComponentModel.DataAnnotations;

namespace DemirorenBlog.Domains.Domain
{
    public class Posts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        [Display(Name = "Content")]
        public string Content { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
