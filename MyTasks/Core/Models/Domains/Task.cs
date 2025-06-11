using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTasks.Core.Models.Domains
{
    [Table("Tasks")]
    public class Task
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Tytuł może mieć maksymalnie 50 znaków.")]
        [Required(ErrorMessage = "Pole Tytuł jest wymagane.")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [MaxLength(250, ErrorMessage = "Opis może mieć maksymalnie 250 znaków.")]
        [Required(ErrorMessage = "Pole Opis jest wymagane.")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pole Kategoria jest wymagane.")]
        [Display(Name = "Kategoria")]
        public int? CategoryId { get; set; }

        [Display(Name = "Termin")]
        [RegularExpression("^(0[1-9]|[12][0-9]|3[01])-(0[13578]|1[02])-([0-9]{4})$|^(0[1-9]|[12][0-9]|30)-(0[469]|11)-([0-9]{4})$|^(0[1-9]|1[0-9]|2[0-8])-02-([0-9]{4})$|^(29)-02-(([0-9]{2}(0[48]|[2468][048]|[13579][26]))|((0[48]|[2468][048]|[13579][26])00))$", ErrorMessage = "Wpisano błędną datę.")]
        public string Term { get; set; }

        [Display(Name = "Zrealizowane")]
        public bool IsExecuted { get; set; }

        public string UserId { get; set; }

        public Category Category { get; set; }

        public ApplicationUser User { get; set; }
    }
}
