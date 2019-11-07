using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DemirorenBlog.Data;
using DemirorenBlog.Domains.Domain;
using DemirorenBlog.Models.AuthsModel;
using DemirorenBlog.Models.PostModels;
using DemirorenBlog.Web.ApiClient;
using DemirorenBlog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemirorenBlog.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IApiService _apiService;
        private readonly DomainContext _domainContext;
        private string apiUrl = "https://localhost:44323/api";
        public PostController(IApiService apiService,DomainContext domainContext)
        {
            _apiService = apiService;
            _domainContext = domainContext;
        }

        [HttpGet]
       /// [Authorize]
        public ActionResult Index()
        {
            var post = this.User;
            if (post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid) == null)
            {
                return RedirectToAction("Index","Home");
            }
            var token = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            var UserId = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var model = new PostIndexModel();
            model.UserId = Convert.ToInt16(UserId);
            var getUri = string.Format("{0}/{1}", apiUrl, "post/list");
            model.Posts = _apiService.GetMethod<List<PostViewModel>>(getUri,headers);
            return View(model);
        }
        
        [HttpGet]
        [Route("[controller]/[action]")]
        public ActionResult Create()
        {
            var post = this.User;
            if(post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid) == null)
            {
                return RedirectToAction("Index");
            }
            
            var token = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            var UserId = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value;
            
            return View();
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public ActionResult Create(PostResponseModel model)
        {
            if (string.IsNullOrEmpty(model.Post.Title) || string.IsNullOrEmpty(model.Post.Content))
            {
                return RedirectToAction("Create");
            }
            var post = this.User;
            var token = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            var UserId = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            model.Post.UserId = Convert.ToInt32(UserId);
            var postUri = string.Format("{0}/{1}", apiUrl, "post/create");
            var result = _apiService.PostMethod<bool>(model, postUri,headers);
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public ActionResult Update(int id)
        {
            var post = this.User;
            var token = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            var Id = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var putUri = string.Format("{0}/{1}", apiUrl, "post/GetByID/" + id);
            var model = _apiService.GetMethod<PostResponseModel>(putUri, headers);
            return View(model);
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public ActionResult Update(int id, PostResponseModel model)
        {
            var post = this.User;
            if (string.IsNullOrEmpty(model.Post.Title) || string.IsNullOrEmpty(model.Post.Content))
            {
                return RedirectToAction("Update");
            }
            var token = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var putUri = string.Format("{0}/{1}", apiUrl, "post/Update/" + id);
            var result = _apiService.PutMethod<bool>(model, putUri, headers);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public ActionResult Delete(int id)
        {
            var post = this.User;
            var token = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var deleteUri = string.Format("{0}/{1}", apiUrl, "post/delete/" + id);
            var result = _apiService.DeleteMethod<bool>(deleteUri,headers);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Search(String word)
        {
            var post = this.User;
            var token = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            var UserId = post.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            var model = new PostIndexModel();
            model.UserId = Convert.ToInt32(UserId);
            var getUri = string.Format("{0}/{1}", apiUrl, "post/search?Word=" + word);
            model.Posts = _apiService.GetMethod<List<PostViewModel>>(getUri,headers);
            if (model.Posts == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
