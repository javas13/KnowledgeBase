using Microsoft.AspNetCore.Mvc;
using StudyTodocups.Data;
using StudyTodocups.Models;

namespace StudyTodocups.Controllers
{
    public class NavigationController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NavigationController(ApplicationDbContext db) 
        {
            _db = db;
        }
        public PartialViewResult Index()
        {
            IEnumerable<Article> articles = _db.Articles.ToList();
            return PartialView("~/Views/Shared/_navigationPartialView.cshtml", articles);
        }
    }
}
