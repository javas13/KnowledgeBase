using StudyTodocups.Models;
namespace StudyTodocups.ViewModels
{
    public class ArticleChangeVM
    {
        public Article Article { get; set; }
        public IEnumerable<Module> Modules { get; set; }
        public Module Module { get; set; }
    }
}
