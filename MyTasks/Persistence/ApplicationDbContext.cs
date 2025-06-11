using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using System.Threading.Tasks;

namespace MyTasks.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
        IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
    {
        public DbSet<Core.Models.Domains.Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        public async Task<int> SaveChangesAsync() =>  await base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Core.Models.Domains.Task>().ToTable("Tasks");
        }
    }
}
