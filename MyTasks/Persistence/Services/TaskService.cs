using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using System.Collections.Generic;

namespace MyTasks.Persistence.Services
{
    public class TaskService(IUnitOfWork unitOfWork) : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IEnumerable<Task> Get(string userId,
            bool isExecuted = false, int categoryId = 0, string title = null) => _unitOfWork.Task.Get(userId, isExecuted, categoryId, title);

        public Task Get(int id, string userId) => _unitOfWork.Task.Get(id, userId);

        public async System.Threading.Tasks.Task Add(Task task)
        {
            _unitOfWork.Task.Add(task);
            await _unitOfWork.Complete();
        }

        public async System.Threading.Tasks.Task Update(Task task)
        {
            _unitOfWork.Task.Update(task);
            await _unitOfWork.Complete();
        }

        public async System.Threading.Tasks.Task Delete(int id, string userId)
        {
            _unitOfWork.Task.Delete(id, userId);
            await _unitOfWork.Complete();
        }

        public async System.Threading.Tasks.Task Finish(int id, string userId)
        {
            _unitOfWork.Task.Finish(id, userId);
            await _unitOfWork.Complete();
        }
    }
}
