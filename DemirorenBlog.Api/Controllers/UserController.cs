using DemirorenBlog.Data;
using DemirorenBlog.Domains.Domain;
using DemirorenBlog.Models.AuthsModel;
using DemirorenBlog.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DemirorenBlog.Api.Controllers
{
    [Route("api/[Controller]/[action]")]
    public class UserController : Controller
    {
        private readonly DomainContext _domainContext;
        public UserController(DomainContext domainContext) {

            _domainContext = domainContext;
        }
        public List<UserViewModel> List()
        {
            var data = _domainContext.Users.ToList();
            var model = data.Select(x =>
            {
                return new UserViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Username = x.Username,
                    Password = x.Password,
                    Photo = x.Photo
                };
            }).ToList();

            return model;
        }

        [HttpPost]
        public bool Create([FromBody] UserViewModel model)
        {
            var data = _domainContext.Users.ToList();
            foreach(var item in data.ToList())
            {
                if(item.Username == model.Username)
                {
                    return false;
                }
            }
            var user = new Users();
            user.Name = model.Name;
            user.Username = model.Username;
            user.Password = model.Password;
            user.Photo = model.Photo;

            _domainContext.Users.Add(user);
            _domainContext.SaveChanges();

            return true;
        }

        [HttpPut("{id}")]
        [Route("api/[Controller]/[action]/{id}")]
        public bool Update(int id, [FromBody] LoginResponseModel model)
        {
            var user = _domainContext.Users.FirstOrDefault(u => u.Id == id);
            if (model == null)
            {
                return false;
            }
            else
            {
                user.Name = model.User.Name;
                user.Photo = model.User.Photo;
                user.Username = model.User.Username;
                user.Password = model.User.Password;
                _domainContext.SaveChanges();
                return true;
            }
        }

        [HttpDelete("{id}")]
        [Route("api/[Controller]/[action]/{id}")]
        public bool Delete(int id)
        {
            var model = _domainContext.Users.FirstOrDefault(u => u.Id == id);
            if (model == null)
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

        [HttpPost]
        public LoginResponseModel GetByLogin([FromBody]LoginModel model)
        {
            var user = _domainContext.Users.FirstOrDefault(u => u.Username == model.Username &&  u.Password == model.Password);
            if (user==null)
            {
                return null;
            }
            var response = new LoginResponseModel();
            var userViewModel = new UserViewModel();           
            userViewModel.Username = user.Username;
            userViewModel.Id = user.Id;
            userViewModel.Name = user.Name;
            userViewModel.Photo = user.Photo;            
            response.User = userViewModel;
            response.Token = GenerateToken(userViewModel);

            return response;
        }

        [HttpGet("{id}")]
        [Route("api/[controller]/[action]/{id}")]
        public LoginResponseModel GetById(int id)
        {
            var user = _domainContext.Users.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return null;
            }
            var response = new LoginResponseModel();
            var userViewModel = new UserViewModel();
            userViewModel.Username = user.Username;
            userViewModel.Id = user.Id;
            userViewModel.Name = user.Name;
            userViewModel.Password = user.Password;
            userViewModel.Photo = user.Photo;
            response.User = userViewModel;
            response.Token = GenerateToken(userViewModel);

            return response;
        }
        
        private string GenerateToken(UserViewModel model)
        {
            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.UniqueName,model.Username),
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
