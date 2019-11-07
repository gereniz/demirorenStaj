using DemirorenBlog.Models.UserModels;

namespace DemirorenBlog.Models.AuthsModel
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public UserViewModel User { get; set; }
    } 
}
