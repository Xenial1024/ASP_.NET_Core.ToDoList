using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MyTasks.Core.Models.Domains
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() => Tasks = [];

        public ICollection<Task> Tasks { get; set; }
    }
}
