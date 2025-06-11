using MyTasks.Core.Repositories;
using System.Threading.Tasks;

namespace MyTasks.Core
{
    public interface IUnitOfWork
    {
        ITaskRepository Task { get; }
        ICategoriesRepository Categories { get; }
        Task Complete();
    }
}
