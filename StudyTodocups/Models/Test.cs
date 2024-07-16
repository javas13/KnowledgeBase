using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTodocups.Models
{
    [Table("tests")]
    public class Test
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        [Column("module_id")]
        public int ModuleId { get; set; }
    }
}
