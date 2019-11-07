using System;

namespace DemirorenBlog.Models.UserModels
{
    [Serializable]
    public class UserViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
