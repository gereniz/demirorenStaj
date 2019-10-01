using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemirorenBlog.Api.Models.CategoryModels;
using DemirorenBlog.Data;
using DemirorenBlog.Domains.Domain;
using Microsoft.AspNetCore.Mvc;

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

        public bool Index()
        {

            return true;
        }


        [HttpPost]
        public bool Create(CategoryVievModel model)
        {
            var category = new Category();
            category.Name = model.Name;

            _domainContext.Categories.Add(category);
            _domainContext.SaveChanges();




            return true;

        }


        public List<Category> List()
        {

            var categories = _domainContext.Categories.ToList();

            return categories;
        }

        [HttpDelete("{id}")]
        [Route("api/[controller]/[action]/{id}")]
        public bool Delete(int id)
        {
            var model = _domainContext.Categories.FirstOrDefault(p => p.Id == id);
            if (model == null)
            {
                return false;
            }
            else
            {
                _domainContext.Categories.Remove(model);
                _domainContext.SaveChanges();
                return true;
            }
        }
    }
}