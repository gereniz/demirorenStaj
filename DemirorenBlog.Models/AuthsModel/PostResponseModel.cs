using DemirorenBlog.Models.CategoryModels;
using DemirorenBlog.Models.PostModels;

namespace DemirorenBlog.Models.AuthsModel
{
    public class PostResponseModel
    {
        public string Token { get; set; }
        public PostViewModel Post { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
