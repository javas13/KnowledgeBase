using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTodocups.Models
{
    [Table("articles")]
    public class Article
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public string Text { get; set; }
        [Column("is_video")]
        public bool IsVideo { get; set; }
        [Column("module_id")]
        public int ModuleId { get; set; }
    }
}
