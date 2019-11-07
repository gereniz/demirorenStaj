using System.Collections.Generic;
using System.Linq;
using DemirorenBlog.Models.PostModels;
using DemirorenBlog.Web.ApiClient;
using DemirorenBlog.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenBlog.Web.Controllers
{
    public class MagazineController : Controller
    {
        private readonly IApiService _apiService;
        private string apiUrl = "https://localhost:44323/api";

        public MagazineController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index(int page)
        {
            var model = new HomeModel();
            List<PostViewModel> list = new List<PostViewModel>();
            List<PostViewModel> liste = new List<PostViewModel>();
            int pageSize = 2;
            int skipSize = (page * pageSize) - pageSize;

            var getUri = string.Format("{0}{1}", apiUrl, "/post/list");
            model.Posts = _apiService.GetMethod<List<PostViewModel>>(getUri);
            foreach (var item in model.Posts)
            {
                if (item.CategoryId == 3)
                {
                    liste.Add(item);
                }

            }
            var data = liste.Skip(skipSize).Take(pageSize);
            int topicCount = liste.Count();
            int pageCount = topicCount / pageSize;
            if (topicCount % pageSize != 0)
            {
                pageCount += 1;
            }

            ViewBag.pageingCount = pageCount;

            foreach (var item in data)
            {
                list.Add(item);
            }
            return View(list);
        }
    }
}