using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyTodocups.Models
{
    [Table("possible_answers")]
    public class PossibleAnswer
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
        [Column("quest_id")]
        public int? QuestId { get; set; }
        [Column("is_correct")]
        public bool IsCorrect { get; set; }
        public int Test_id { get; set; }
    }
}
