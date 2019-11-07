using DemirorenBlog.Models.PostModels;
using System.Collections.Generic;

namespace DemirorenBlog.Web.Models
{
    public class PostIndexModel
    {
        public int UserId { get; set; }
        public string Word { get; set; }
        public List<PostViewModel> Posts { get; set; }        
    }
}
