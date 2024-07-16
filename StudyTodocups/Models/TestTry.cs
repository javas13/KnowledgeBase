using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTodocups.Models
{
    [Table("test_try")]
    public class TestTry
    {
        [Key]
        public int Id { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("test_id")]
        public int TestId { get; set; }
    }
}
