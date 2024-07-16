using StudyTodocups.Models;
namespace StudyTodocups.ViewModels
{
    public class NavigationPartialVM
    {
        public IEnumerable<Module> Modules { get; set; }
        public int ArticleId { get; set; }

    }
}
