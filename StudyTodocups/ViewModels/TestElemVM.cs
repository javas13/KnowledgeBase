using StudyTodocups.Models;

namespace StudyTodocups.ViewModels
{
    public class TestElemVM
    {
        public IEnumerable<QuestionVM>? Questions { get; set; }
        public int Test_id { get; set; }
        public List<Answer>? Answers { get; set; }
        public bool? isTestWrong { get; set; }
        public string? TestError { get; set; }
        //public Question Question { get; set; }
        //public IEnumerable<PossibleAnswer> PossibleAnswers { get; set; }
    }
}
