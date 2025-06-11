using Microsoft.EntityFrameworkCore;
using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;


namespace MyTasks.Persistence.Repositories
{
    public class CategoriesRepository(IApplicationDbContext context) : ICategoriesRepository
    {
        readonly IApplicationDbContext _context = context;
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<bool> CategoryExists(string name, string userId)
        {
            var query = _context.Categories
                .Where(c => c.Name == name && (c.UserId == userId || c.UserId == null));

            return await query.AnyAsync();
        }
        
        public async Task<IEnumerable<Category>> GetCategories(string userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task Add(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public Task Edit(Category category)
        {
            _context.Categories.Update(category);
            return Task.CompletedTask;
        }

        public async Task Delete(Category category)
        {
            category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == category.Id && c.UserId == category.UserId);

            if (category != null)
                _context.Categories.Remove(category);
        }
    }
}