using MyTasks.Core.Models.Domains;
using System.Collections.Generic;

namespace MyTasks.Core.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<Task> Get(string userId,
            bool isExecuted = false, int categoryId = 0, string title = null);
        Task Get(int id, string userId);
        void Add(Task task);
        void Update(Task task);
        void Delete(int id, string userId);
        void Finish(int id, string userId);
    }
}
