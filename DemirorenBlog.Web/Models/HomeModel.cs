using DemirorenBlog.Models.CategoryModels;
using DemirorenBlog.Models.PostModels;
using System;
using System.Collections.Generic;

namespace DemirorenBlog.Web.Models
{
    public class HomeModel
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public string Logo { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }
}
