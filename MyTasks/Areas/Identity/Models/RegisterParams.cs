using Microsoft.AspNetCore.Identity;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;

namespace MyTasks.Areas.Identity.Models
{
    public class RegisterParams
    {
        public UserManager<ApplicationUser> UserManager;
        public SignInManager<ApplicationUser> SignInManager;
        public ICategoriesService CategoriesService;
    }
}
