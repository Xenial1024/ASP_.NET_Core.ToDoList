using MyTasks.Core.Models.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace MyTasks.Core.Services
{
    public interface ICategoriesService
    {
        Task<bool> CategoryExists(string name, string userId); 
        Task<IEnumerable<Category>> GetCategories(string userId);
        Task Add(Category category);
        Task Edit(Category category);
        Task Delete(Category category);
    }
}