﻿using Microsoft.EntityFrameworkCore;
using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MyTasks.Persistence.Repositories
{
    public class TaskRepository(IApplicationDbContext context) : ITaskRepository
    {
        readonly IApplicationDbContext _context = context;

        public IEnumerable<Task> Get(string userId, 
            bool isExecuted = false, int categoryId = 0, string title = null)
        {
            var tasks = _context.Tasks
                .Include(x => x.Category)
                .Where(x => x.UserId == userId &&
                    x.IsExecuted == isExecuted);

            if (categoryId != 0)
                tasks = tasks.Where(x => x.CategoryId == categoryId);

            if (!string.IsNullOrWhiteSpace(title))
                tasks = tasks.Where(x => x.Title.Contains(title));

            return [.. tasks.OrderBy(x => x.Term)];
        }

        public Task Get(int id, string userId)
        {
            Task task = _context.Tasks
                .Single(x => x.Id == id && x.UserId == userId);

            return task;
        }

        public void Add(Task task) => _context.Tasks.Add(task);

        public void Update(Task task)
        {
            Task taskToUpdate = _context.Tasks.Single(x => x.Id == task.Id);

            taskToUpdate.CategoryId = task.CategoryId;
            taskToUpdate.Description = task.Description;
            taskToUpdate.IsExecuted = task.IsExecuted;
            taskToUpdate.Term = task.Term;
            taskToUpdate.Title = task.Title;

            _context.Tasks.Update(taskToUpdate);
        }

        public void Delete(int id, string userId)
        {
            Task taskToDelete = _context.Tasks
                .Single(x => x.Id == id && x.UserId == userId);

            _context.Tasks.Remove(taskToDelete);
        }

        public void Finish(int id, string userId)
        {
            Task taskToUpdate = _context.Tasks
               .Single(x => x.Id == id && x.UserId == userId);

            taskToUpdate.IsExecuted = true;
        }
    }
}
