using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTodocups.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле \"Email\" пустое")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Поле \"Пароль\" пустое")]
        public string Password { get; set; }
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
