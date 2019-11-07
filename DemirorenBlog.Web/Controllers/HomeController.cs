using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemirorenBlog.Models.PostModels;
using DemirorenBlog.Web.ApiClient;
using DemirorenBlog.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;
        private string apiUrl = "https://localhost:44323/api";

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }
        
        public IActionResult Index(int page)
        {
            var model = new HomeModel();
            int pageSize = 2;
            int skipSize = (page * pageSize)-pageSize;

            var getUri = string.Format("{0}{1}", apiUrl, "/post/list");
            model.Posts = _apiService.GetMethod<List<PostViewModel>>(getUri);
            var data = model.Posts.Skip(skipSize).Take(pageSize);

            int topicCount = model.Posts.Count();
            int pageCount = topicCount / pageSize;
            if(topicCount % pageSize != 0)
            {
                pageCount += 1;
            }
            ViewBag.pageingCount = pageCount;

            List<PostViewModel> list = new List<PostViewModel>();
            foreach (var item in data)
            {
                list.Add(item);
            }
            return View(list);
        }

        public ActionResult Search(string word)
        {
            var model = new HomeModel();
            var getUri = string.Format("{0}/{1}", apiUrl, "post/search?Word=" + word);
            model.Posts = _apiService.GetMethod<List<PostViewModel>>(getUri);
            if(model.Posts == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
            
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


