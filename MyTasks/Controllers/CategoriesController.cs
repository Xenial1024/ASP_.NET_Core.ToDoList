using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using MyTasks.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyTasks.Controllers
{
    [Authorize]
    public class CategoriesController(ICategoriesService categoriesService) : Controller
    {
        readonly ICategoriesService _categoriesService = categoriesService;
        static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public async Task<IActionResult> Categories()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Category> categories = await _categoriesService.GetCategories(userId);
            return View(categories);
        }

        public async Task<IActionResult> GetCategoriesTable()
        {
            string userId = User.GetUserId();
            IEnumerable<Category> categories = await _categoriesService.GetCategories(userId);
            return PartialView("_CategoriesTable", categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return Json(new { success = false, message = "Nazwa nie może być pusta." });
                if (name.Length>50)
                    return Json(new { success = false, message = "Nazwa może mieć maksymalnie 50 znaków." });
                string userId = User.GetUserId();
                Category category = new ()
                {
                    Name = name,
                    UserId = userId
                };

                await _categoriesService.Add(category);
                return Json(new { success = true, id = category.Id, name = category.Name });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Błąd podczas dodawania kategorii {CategoryName}.", name);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name)
        {
            try
            {
                var userId = User.GetUserId();

                IEnumerable<Category> categories = await _categoriesService.GetCategories(userId);
                Category category = categories.FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return Json(new { success = false, message = "Nie znaleziono kategorii." });
                }

                category.Name = name;
                await _categoriesService.Edit(category);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Błąd podczas edycji kategorii o id {CategoryId}. Nie udało się zmienić nazwy na: {CategoryName}.", id, name);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userId = User.GetUserId();
                IEnumerable<Category> categories = await _categoriesService.GetCategories(userId);
                Category category = categories.FirstOrDefault(c => c.Id == id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Nie znaleziono kategorii." });
                }
                await _categoriesService.Delete(category);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Błąd podczas usuwania kategorii o id {CategoryId}.", id);
                throw;
            }
        }
    }
}
