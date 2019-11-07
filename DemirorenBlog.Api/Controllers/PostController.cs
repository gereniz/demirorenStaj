using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DemirorenBlog.Data;
using DemirorenBlog.Domains.Domain;
using DemirorenBlog.Models.AuthsModel;
using DemirorenBlog.Models.PostModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace DemirorenBlog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PostController : Controller
    {
        private readonly DomainContext _domainContext;
        private readonly IMemoryCache _memoryCache;
        const string key = "postListKey";
        public PostController(DomainContext domainContext , IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _domainContext = domainContext;
        }

        [HttpPost]
        public bool Create([FromBody] PostResponseModel model)
        {
            var post = new Posts();
            post.Title = model.Post.Title;
            post.Content = model.Post.Content;
            post.UserId = model.Post.UserId;
            post.CategoryId = model.Category.Id;
            post.CreateDate = DateTime.Now;

            _domainContext.Posts.Add(post);
            _domainContext.SaveChanges();

            return true;
        }

        public List<PostViewModel> List()
        {
           if(_memoryCache.TryGetValue(key, out List<PostViewModel> list))
           {
               return list;
           }
           var data = _domainContext.Posts.ToList();
           var model = data.Select(x =>
           {
               return new PostViewModel()
               {
                   Id = x.Id,
                   Title = x.Title,
                   Content = x.Content,
                   UserId = x.UserId,
                   CategoryId = x.CategoryId,
                    
               };
           }).ToList();
           _memoryCache.Set(key, model, new MemoryCacheEntryOptions
           {
                AbsoluteExpiration = DateTime.Now.AddSeconds(5),
                Priority = CacheItemPriority.Normal
           });

           return model;
        }

        [HttpPut("{id}")]
        [Route("api/[controller]/[action]/{id}")]
        public bool Update(int id, [FromBody] PostResponseModel model)
        {
            var post = _domainContext.Posts.FirstOrDefault(p => p.Id == id);
            if (model == null) {
                return false;
            }
            else
            {
                post.Title = model.Post.Title;
                post.Content = model.Post.Content;
                post.CategoryId = model.Category.Id;
                
                _domainContext.SaveChanges();
                _memoryCache.Remove(key);
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
                _memoryCache.Remove(key);
                return true;
            }         
        }

        [HttpGet("{id}")]
        [Route("api/[controller]/[action]/{id}")]
        public PostResponseModel GetByID(int id)
        {
            var post = _domainContext.Posts.FirstOrDefault(p => p.Id == id);
            var response = new PostResponseModel();
            var model = new PostViewModel();
            model.Id = post.Id;
            model.Title = post.Title;
            model.Content = post.Content;
            model.UserId = post.UserId;
            model.CategoryId = post.CategoryId;
            response.Post = model;
            response.Token = GenerateToken(model);
            return response;
        }

        [HttpGet]
        public List<PostViewModel> Search(string word)
        {
            List<PostViewModel> list = new List<PostViewModel>();
            var data = _domainContext.Posts.ToList();
            var model = data.Select(x =>
            {
                return new PostViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    UserId = x.UserId,
                    CategoryId = x.CategoryId,
                    CreateDate = x.CreateDate,
                };
            }).ToList();
            if (word == null)
            {
                return null;
            }
            foreach (var item in model)
            {
                if (item.Title.Contains(word) || item.Content.Contains(word))
                {
                    list.Add(item);   
                }
            }
            return list;
        }

        private string GenerateToken(PostViewModel model)
        {
            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.UniqueName,model.Title),                      
                new Claim(JwtRegisteredClaimNames.NameId,Guid.NewGuid().ToString())
            };

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GüştaGerenizSüleymanDemirelÜniversitesiDemirorenTeknoloji"));
            var token = new JwtSecurityToken(
                issuer: "",
                audience: "",
                claims: someClaims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}