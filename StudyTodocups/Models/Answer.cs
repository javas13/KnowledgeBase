using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTodocups.Models
{
    [Table("answers")]
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        [Column("possible_answer_id")]
        public int PossibleAnswerId { get; set; }
        [Column("is_correct")]
        public bool IsCorrect { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("quest_id")]
        public int? Quest_Id { get; set; }
        [Column("test_id")]
        public int Test_id { get; set; }
    }
}
