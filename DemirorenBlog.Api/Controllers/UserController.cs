using DemirorenBlog.Api.Models.PostModels;
using DemirorenBlog.Data;
using DemirorenBlog.Domains.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DemirorenBlog.Api.Controllers
{

    [Route("api/[Controller]/[action]")]
    public class UserController : Controller
    {
        private readonly DomainContext _domainContext;
        public UserController(DomainContext domainContext) {

            _domainContext = domainContext;
        }
       
        [HttpPost]
        public bool Create( [FromBody] UserViewModel model)
        {
            var user = new Users();
            user.Name = model.Name;
            user.Photo = model.Photo;

            _domainContext.Users.Add(user);
            _domainContext.SaveChanges();

            return true;
        }

        public List<Users> List()
        {
            var model = _domainContext.Users.OrderByDescending(u => u.Id).ToList();
            return model;
        }

        [Route("api/[Controller]/[action]/{id}")]
        [HttpPut("{id}")]

        public bool Update(int id, [FromBody] UserViewModel model)
        {
            var user = _domainContext.Users.FirstOrDefault(u => u.Id == id);
            if (model == null)
            {
                return false;
            }
            else
            {
                user.Name = model.Name;
                user.Photo = model.Photo;
                _domainContext.SaveChanges();
                return true;
            }

        }

        [Route("api/[Controller]/[action]/{id}")]
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var model = _domainContext.Users.FirstOrDefault(u => u.Id == id);
            if(model == null)
            {
                return false;
            }
            else
            {
                _domainContext.Users.Remove(model);
                _domainContext.SaveChanges();
                return true;
            }
        }
    }
}
