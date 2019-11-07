using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DemirorenBlog.Models.AuthsModel;
using DemirorenBlog.Web.ApiClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenBlog.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IApiService _apiService;
        private string apiUrl = "https://localhost:44323/api";
        public LoginController(IApiService apiService )
        {
            _apiService = apiService;
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public ActionResult Index(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return RedirectToAction("Index");
            }
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;
            var postUri = string.Format("{0}/{1}", apiUrl, "user/GetByLogin");
            var result = _apiService.PostMethod<LoginResponseModel>(model, postUri);
            if (result != null)
            {
                identity = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Name, result.User.Name),
                    new Claim(ClaimTypes.Role, "user"),
                    new Claim(ClaimTypes.NameIdentifier,result.User.Id.ToString()),
                    new Claim(ClaimTypes.Sid,result.Token),
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticated = true;
            }
            else
            {
                return RedirectToAction("Update");
            }
            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult Update()
        {
            var user = this.User;
            if(user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid) == null)
            {
                return RedirectToAction("Index");
            }
            var token = user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid).Value;
            var Id = user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var putUri = string.Format("{0}/{1}", apiUrl, "user/GetById/" + Id);
            var model = _apiService.GetMethod<LoginResponseModel>(putUri, headers);
            if(model == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public ActionResult Update(int id ,LoginResponseModel model)
        {
            var user = this.User;
            if (string.IsNullOrEmpty(model.User.Name) || string.IsNullOrEmpty(model.User.Username) )
            {
                return RedirectToAction("Update");
            }
            if (user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid) == null)
            {
                return RedirectToAction("Update");
            }
            var token = user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var putUri = string.Format("{0}/{1}", apiUrl, "user/Update/" + id);
            var result = _apiService.PutMethod<bool>(model, putUri, headers);
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public ActionResult Delete(int id)
        {
            var user = this.User;
            var token = user.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var deleteUri = string.Format("{0}/{1}", apiUrl, "user/delete/" + id);
            var result = _apiService.DeleteMethod<bool>( deleteUri, headers);
            
            return RedirectToAction("Index","Home");
        }
        public ActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
