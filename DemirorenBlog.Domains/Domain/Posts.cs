using System;
using System.Collections.Generic;
using System.Text;

namespace DemirorenBlog.Domains.Domain
{
    public class Posts
    {
     
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
