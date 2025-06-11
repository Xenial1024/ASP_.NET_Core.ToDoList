using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;
using System.Collections.Generic;

namespace MyTasks.Core.ViewModels
{
    public class TasksViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public FilterTasks FilterTasks { get; set; }
    }
}
