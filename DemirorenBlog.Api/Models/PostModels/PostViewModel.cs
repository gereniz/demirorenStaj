using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemirorenBlog.Api.Models.PostModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }

    }
}
