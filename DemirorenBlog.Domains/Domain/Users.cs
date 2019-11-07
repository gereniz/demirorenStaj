using System.ComponentModel.DataAnnotations;

namespace DemirorenBlog.Domains.Domain
{
   public class Users
   {
        public int Id { get; set; }public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
   }
}
