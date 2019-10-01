using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DemirorenBlog.Api.Models.PostModels;
using DemirorenBlog.Data;
using DemirorenBlog.Domains.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemirorenBlog.Api.Controllers
{

    [Route("api/[controller]/[action]")]
    public class PostController : Controller
    {
        private readonly DomainContext _domainContext;
        public PostController(DomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        [HttpPost]
        public bool Create([FromBody] PostViewModel model)
        {


            var post = new Posts();
            post.Title = model.Title;
            post.Content = model.Content;
            post.UserId = 1;
            post.CreateDate = DateTime.Now;

            _domainContext.Posts.Add(post);
            _domainContext.SaveChanges();


            return true;
        }

        public List<Posts> List()
        {
            var model = _domainContext.Posts.OrderByDescending(p => p.Id).ToList();
            return model;

        }

        [Route("api/[controller]/[action]/{id}")]
        [HttpPut("{id}")]
        public bool Update(int id, [FromBody] PostViewModel model)
        {
            var post = _domainContext.Posts.FirstOrDefault(p => p.Id == id);
            if (model == null) {
                return false;
            }
            else
            {
                post.Title = model.Title;
                post.Content = model.Content;
                _domainContext.SaveChanges();
                return true;
            }


        }


        [HttpDelete("{id}")]
        [Route("api/[controller]/[action]/{id}")]
        public bool Delete(int id)
        {
            var model = _domainContext.Posts.FirstOrDefault(p => p.Id == id );
            if(model == null)
            {
                return false;
            }
            else
            {
                _domainContext.Posts.Remove(model);
                _domainContext.SaveChanges();
                return true;
             
        }
            

        }
    }
}