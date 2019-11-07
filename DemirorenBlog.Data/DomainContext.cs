using DemirorenBlog.Domains.Domain;
using Microsoft.EntityFrameworkCore;

namespace DemirorenBlog.Data
{
    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet <Users> Users { get; set; }
        public DbSet <Posts> Posts{ get; set; }       
        public DbSet <Category> Category { get; set; }
    }
}
