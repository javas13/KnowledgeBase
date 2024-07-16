using Microsoft.AspNetCore.Mvc;
using StudyTodocups.Data;
using StudyTodocups.Models;
using System.Diagnostics;
using StudyTodocups.Middleware;
using System.Linq;
using StudyTodocups.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace StudyTodocups.Controllers
{
    [SiteAuthorize()]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor httpContextAccessor;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _db = db;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            List<Module> modulesLst = _db.Modules.ToList();
            List<ModuleForList> moduleList = new List<ModuleForList>();
            foreach(var p in modulesLst)
            {
                var videosCnt = (from art in _db.Articles where art.ModuleId == p.Id && art.IsVideo == true select art.Id).Count();
                var articlesCnt = (from art in _db.Articles where art.ModuleId == p.Id && art.IsVideo != true select art.Id).Count();
                ModuleForList moduleForList = new ModuleForList()
                {
                    Id = p.Id,
                    Description = p.Description,
                    ArticlesCount = articlesCnt,
                    Name = p.Name,
                    VideosCount = videosCnt,
                };
                moduleList.Add(moduleForList);
            }
            //IEnumerable<Module> modules = _db.Modules.ToList();
            return View(moduleList);
        }
        [HttpGet]
        public async Task<IActionResult> Module(int id)
        {
            int? userId = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            Module firstModule = await _db.Modules.FirstOrDefaultAsync();
            if(firstModule.Id != id)
            {
                Test test = await _db.Tests.Where(i => i.ModuleId < id).OrderBy(i => i.ModuleId).LastOrDefaultAsync();
                TestTry testTry = await _db.TestTries.Where(i => i.UserId == userId && i.TestId == test.Id).FirstOrDefaultAsync();
                if (testTry == null)
                {
                    NotAccessErrorVM notAccessVM = new NotAccessErrorVM()
                    {
                        ErrorMes = "Данный модуль вам не доступен! Пройдите предыдущие модули чтобы получить доступ к текущему."
                    };
                    List<string> breads = new List<string>();
                    breads.Add("Ошибка");
                    ViewBag.ModuleName = breads;
                    return View("NotAccess", notAccessVM);
                }
            }
            
            var firstArticle =_db.Articles.Where(i=> i.ModuleId == id).Select(i=> i.Id).Min();        
            return RedirectToAction("Article", new { modul_id = id, art_id = firstArticle});
        }
        [HttpGet]
        [Route("/Article/modul_id_{modul_id}/art_id_{art_id}")]
        public async Task<IActionResult> Article(int modul_id, int art_id)
        {
            int? userId = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            List<string> breads = new List<string>();
            Module firstModule = await _db.Modules.FirstOrDefaultAsync();
            if (firstModule.Id != modul_id)
            {
                Test test = await _db.Tests.Where(i => i.ModuleId < modul_id).OrderBy(i => i.ModuleId).LastOrDefaultAsync();
                TestTry testTry = await _db.TestTries.Where(i => i.UserId == userId && i.TestId == test.Id).FirstOrDefaultAsync();
                if (testTry == null)
                {
                    NotAccessErrorVM notAccessVM = new NotAccessErrorVM()
                    {
                        ErrorMes = "Данный модуль вам не доступен! Пройдите предыдущие модули чтобы получить доступ к текущему."
                    };
                    breads.Add("Ошибка");
                    ViewBag.ModuleName = breads;
                    return View("NotAccess", notAccessVM);
                }
            }

            ViewBag.CurrentModuleId = modul_id;
            ViewBag.CurrentArticleId = art_id;
            int TestID = 0;
            var nextID = await _db.Articles.Where(i => i.ModuleId == modul_id).OrderBy(i => i.Id).Where(i => i.Id > art_id).Select(i => i.Id).FirstOrDefaultAsync();
            var previousID = await _db.Articles.Where(i => i.ModuleId == modul_id).OrderBy(i => i.Id).Where(i => i.Id < art_id).Select(i => i.Id).LastOrDefaultAsync();
            var currentArt = await _db.Articles.Where(i => i.ModuleId == modul_id).Where(x => x.Id == art_id).FirstOrDefaultAsync();
            var nextModul = await _db.Modules.Where(i => i.Id > modul_id).OrderBy(i => i.Id).Select(i=> i.Id).FirstOrDefaultAsync();
            var prevModul = await _db.Modules.Where(i => i.Id < modul_id).OrderBy(i => i.Id).Select(i => i.Id).LastOrDefaultAsync();
            var currentModule = await _db.Modules.Where(i=> i.Id == modul_id).Select(i => i.Name).FirstOrDefaultAsync();
            

            if (currentArt == null)
            {
                return NotFound();
            }

            if(nextID == 0)
            {
               Test testOfModule = await _db.Tests.Where(i => i.ModuleId == modul_id).FirstOrDefaultAsync();
               TestTry testTry = await _db.TestTries.Where(i => i.UserId == userId && i.TestId == testOfModule.Id).FirstOrDefaultAsync();
                if(testTry == null)
                {
                    TestID = testOfModule.Id; 
                }
            }
            //ViewBag.ModuleName = currentArt.Name;
           
            breads.Add(currentModule);
            breads.Add(currentArt.Name);
            ViewBag.ModuleName = breads;

            if (nextModul == null)
            {
                nextModul = 0;
            }
            else if (prevModul == null)
            {
                prevModul = 0;
            }
            ModuleVM moduleVM = new ModuleVM()
            {
                NextArticle = nextID,
                PreviousArticle = previousID,
                ArticleModule = currentArt,
                ModuleId = modul_id,
                NextModuleId = nextModul,
                PreviousModuleId = prevModul,
                TestId = TestID
            };
            
            return View("Privacy", moduleVM);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}