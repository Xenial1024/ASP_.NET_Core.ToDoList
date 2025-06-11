using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using MyTasks.Core.ViewModels;
using MyTasks.Persistence.Extensions;
using System;

namespace MyTasks.Controllers
{
    [Authorize]
    public class TaskController(ITaskService taskService, ICategoriesService categoriesService) : Controller
    {
        readonly ITaskService _taskService = taskService;
        readonly ICategoriesService _categoriesService = categoriesService;
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();


        public async System.Threading.Tasks.Task<IActionResult> Tasks()
        {
            string userId = User.GetUserId();

            TasksViewModel vm = new()
            {
                FilterTasks = new(),
                Tasks = _taskService.Get(userId),
                Categories = await _categoriesService.GetCategories(userId)
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Tasks(TasksViewModel viewModel)
        {
            if (viewModel.FilterTasks.CategoryId == 0)
                viewModel.FilterTasks.CategoryId = null; //to jest potrzebne, żeby wyświetlał się komunikat "Pole Kategoria jest wymagane."

            var userId = User.GetUserId();

            var tasks = _taskService.Get(userId,
                viewModel.FilterTasks.IsExecuted,
                viewModel.FilterTasks.CategoryId ?? 0,
                viewModel.FilterTasks.Title);

            return PartialView("_TasksTable", tasks);
        }

        public async System.Threading.Tasks.Task<IActionResult> Task(int id = 0)
        {
            var userId = User.GetUserId();

            var task = id == 0 ?
                new Task { Id = 0, UserId = userId, Term = string.Empty } :
                _taskService.Get(id, userId);
 
            var vm = new TaskViewModel
            {
                Task = task,
                Heading = id == 0 ? 
                    "Dodawanie nowego zadania" : "Edytowanie zadania",
                Categories = await _categoriesService.GetCategories(userId)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Task(Task task)
        {
            var userId = User.GetUserId();
            task.UserId = userId;

            if (!ModelState.IsValid)
            {
                var vm = new TaskViewModel
                {
                    Task = task,
                    Heading = task.Id == 0 ?
                    "Dodawanie nowego zadania" : "Edytowanie zadania",
                    Categories = await _categoriesService.GetCategories(userId)
                };

                return View("Task", vm);
            }

            if (task.Id == 0)
                await _taskService.Add(task);
            else
                await _taskService.Update(task);            

            return RedirectToAction("Tasks");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = User.GetUserId();
                await _taskService.Delete(id, userId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Błąd podczas usuwania zadania.");
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Finish(int id)
        {
            try
            {
                var userId = User.GetUserId();
                await _taskService.Finish(id, userId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Błąd podczas oznaczania zadania jako ukończone.");
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true });
        }
    }
}
