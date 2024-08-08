using ContactManager.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Server.Contexts
{
    public class ApplicationContext : IdentityDbContext<Contact>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("identity");
        }
    }
}
