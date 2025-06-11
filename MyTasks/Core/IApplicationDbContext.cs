using Microsoft.EntityFrameworkCore;
using MyTasks.Core.Models.Domains;
using System.Threading.Tasks;

namespace MyTasks.Core
{
    public interface IApplicationDbContext
    {
        DbSet<Models.Domains.Task> Tasks { get; set; }
        DbSet<Category> Categories { get; set; }
        Task<int> SaveChangesAsync();
    }
}
