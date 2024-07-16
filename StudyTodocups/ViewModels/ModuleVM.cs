using StudyTodocups.Models;
using StudyTodocups.ViewModels;
namespace StudyTodocups.ViewModels
{
    public class ModuleVM
    {
        public Article ArticleModule { get; set; }
        public int PreviousArticle { get; set; }
        public int NextArticle { get; set; }
        public int ModuleId { get; set; }
        public int? NextModuleId { get; set; }
        public int? PreviousModuleId { get; set; }
        public int TestId { get; set; }
    }
}
