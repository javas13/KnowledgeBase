using StudyTodocups.Models;

namespace StudyTodocups.ViewModels
{
    public class QuestionVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public int Test_Id { get; set; }
        public IEnumerable<PossibleAnswer>? Answers { get; set; }
    }
}
