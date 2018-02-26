using Microsoft.AspNet.Identity.EntityFramework;

namespace Avelango.Models
{
    public class ApplicationDbContext : IdentityDbContext<Application.ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}