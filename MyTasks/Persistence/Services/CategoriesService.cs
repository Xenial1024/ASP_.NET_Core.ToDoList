using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace MyTasks.Persistence.Services
{
    public class CategoriesService(IUnitOfWork unitOfWork) : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> CategoryExists(string name, string userId) => await _unitOfWork.Categories.CategoryExists(name, userId);

        public async Task<IEnumerable<Category>> GetCategories(string userId) => await _unitOfWork.Categories.GetCategories(userId);


        public async Task Add(Category category)
        {
            if (await CategoryExists(category.Name, category.UserId))
                throw new InvalidOperationException($"Kategoria '{category.Name}' już istnieje.");

            await _unitOfWork.Categories.Add(category);
            await _unitOfWork.Complete();
        }

        public async Task Edit(Category category)
        {
            await _unitOfWork.Categories.Edit(category);
            await _unitOfWork.Complete();
        }

        public async Task Delete(Category category)
        {
            await _unitOfWork.Categories.Delete(category);
            await _unitOfWork.Complete();
        }
    }
}
