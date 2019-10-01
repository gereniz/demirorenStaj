using DemirorenBlog.Domains.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemirorenBlog.Data
{
    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet <Users> Users { get; set; }
        public DbSet <Posts> Posts{ get; set; }
        
        public DbSet <Category> Categories { get; set; }

    }
}
