using DemirorenBlog.Models.UserModels;
using DemirorenBlog.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenBlog.Web.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IApiService _apiService;
        private string apiUrl = "https://localhost:44323/api";
        public SignUpController(IApiService apiService)
        {
            _apiService = apiService ;
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return RedirectToAction("Index");
            }
            var postUri = string.Format("{0}/{1}", apiUrl, "user/create");
            var result = _apiService.PostMethod<bool>(model, postUri);
            if(result == false)
            {

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Login");
        }
    }
}