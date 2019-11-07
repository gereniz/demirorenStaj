using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DemirorenBlog.Data
{
    class FakeDomainContextFactory : IDesignTimeDbContextFactory<DomainContext>
    {
        public DomainContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database = DemirorenBlog; Trusted_Connection = True");
            return new DomainContext(optionsBuilder.Options);
        }
    }
}
