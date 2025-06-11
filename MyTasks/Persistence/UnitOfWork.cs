using MyTasks.Core;
using MyTasks.Core.Repositories;
using MyTasks.Persistence.Repositories;
using System.Threading.Tasks;

namespace MyTasks.Persistence
{
    public class UnitOfWork(IApplicationDbContext context) : IUnitOfWork
    {
        readonly IApplicationDbContext _context = context;
        ICategoriesRepository _categories;

        public ITaskRepository Task { get; set; } = new TaskRepository(context);

        public ICategoriesRepository Categories =>
    _categories ??= new CategoriesRepository(_context);

        public async Task Complete() => await _context.SaveChangesAsync();
    }
}
