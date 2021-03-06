﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DemirorenBlog.Data;
using DemirorenBlog.Domains.Domain;
using DemirorenBlog.Models.AuthsModel;
using DemirorenBlog.Models.CategoryModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace DemirorenBlog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly DomainContext _domainContext;
        
        public CategoryController(DomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        [HttpPost]
        public bool Create([FromBody] CategoryViewModel model)
        {
            var category = new Category();
            category.Id = model.Id;
            category.Name = model.Name;

            _domainContext.Category.Add(category);
            _domainContext.SaveChanges();

            return true;
        }
       
        public List<CategoryViewModel> List()
        {

            var data = _domainContext.Category.ToList();
            var model = data.Select(x =>
            {
                return new CategoryViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                };
            }).ToList();

            return model;
        }

        [HttpDelete("{id}")]
        [Route("api/[controller]/[action]/{id}")]
        public bool Delete(int id)
        {
            var model = _domainContext.Category.FirstOrDefault(c => c.Id == id);
            if (model == null)
            {
                return false;
            }
            else
            {
                _domainContext.Category.Remove(model);
                _domainContext.SaveChanges();
                return true;
            }
        }

        [HttpPut("{id}")]
        [Route("api/[controller]/[action]/{id}")]
        public bool Update(int id, [FromBody] CategoryViewModel model)
        {
            var categories = _domainContext.Category.FirstOrDefault(c => c.Id == id);
            if (model == null)
            {
                return false;
            }
            else
            {
                categories.Name = model.Name;

                _domainContext.SaveChanges();
                return true;
            }
        }
    }
}