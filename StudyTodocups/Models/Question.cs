using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTodocups.Models
{
    [Table("questions")]
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public int Test_Id {  get; set; }
        
    }
}
