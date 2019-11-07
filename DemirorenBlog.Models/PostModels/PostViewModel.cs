using System;

namespace DemirorenBlog.Models.PostModels
{
    [Serializable]
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
