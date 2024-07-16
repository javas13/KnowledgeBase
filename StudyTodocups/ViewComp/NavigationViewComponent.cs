using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudyTodocups.Data;
using StudyTodocups.Models;

namespace StudyTodocups.ViewComp
{
    [ViewComponent]
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public NavigationViewComponent(ApplicationDbContext db) 
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            IEnumerable<Module> modules = _db.Modules.ToList();
            return View("_navigationPartialView", modules);
        }
    }
}
