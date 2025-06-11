using MyTasks.Core.Models.Domains;
using System.Collections.Generic;

namespace MyTasks.Core.Services
{
    public interface ITaskService
    {
        IEnumerable<Task> Get(string userId,
            bool isExecuted = false, int categoryId = 0, string title = null);
        Task Get(int id, string userId);
        System.Threading.Tasks.Task Add(Task task);
        System.Threading.Tasks.Task Update(Task task);
        System.Threading.Tasks.Task Delete(int id, string userId);
        System.Threading.Tasks.Task Finish(int id, string userId);
    }
}
