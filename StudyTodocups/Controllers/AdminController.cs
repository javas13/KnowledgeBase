using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyTodocups.Data;
using StudyTodocups.Models;
using System.Net.Http.Headers;
using StudyTodocups.Services;
using StudyTodocups.ViewModels;
using StudyTodocups.Middleware;

namespace StudyTodocups.Controllers
{
    [AdminAuthorize()]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AdminController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor) 
        {
            _db = db;
            this.httpContextAccessor = httpContextAccessor;
        }    
        public IActionResult Index()
        {
            IEnumerable<Module> modules = _db.Modules.ToList();
            return View(modules);
        }
        public async Task<IActionResult> ArticleUpdate(int id)
        {
            var articleChng = await _db.Articles.Where(a => a.Id == id).FirstOrDefaultAsync();
            var articleModule = await _db.Modules.Where(a=> a.Id == articleChng.ModuleId).FirstOrDefaultAsync();
            var moduleList = await _db.Modules.ToListAsync();
            ArticleChangeVM vm = new ArticleChangeVM()
            {
                Article = articleChng,
                Module = articleModule,
                Modules = moduleList
            };
            return View(vm);
        }
        public async Task<IActionResult> ModuleList()
        {
            IEnumerable<Module> modules = await _db.Modules.ToListAsync();
            return View(modules);
        }
        public async Task<IActionResult> ArticleList(int id)
        {
            IEnumerable<Article> articles = await _db.Articles.Where(x=> x.ModuleId == id).ToListAsync();
            return View(articles);
        }
        public IActionResult ArticleElement(int id)
        {
            return View(_db.Articles.Where(x => x.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public IActionResult SaveText(string content, string article_name, string article_descr, string is_video, int module_id)
        {
            Article article = new Article()
            {
                Name = article_name,
                Text = content,
                Description = article_descr,
                ModuleId = module_id
            };
            _db.Articles.Add(article);
            _db.SaveChanges();
            return RedirectToAction("ArticleElement", "Admin", new { id = article.Id });
        }
        [HttpPost]
        public IActionResult UpdateText(string content, string article_name, string article_descr, string is_video, int module_id, int art_id)
        {
            Article article_upd = _db.Articles.Where(i => i.Id == art_id).FirstOrDefault();
            article_upd.Text = content;
            article_upd.Name = article_name;
            article_upd.Description = article_descr;
            article_upd.ModuleId = module_id;
            if(is_video == "on")
            {
                article_upd.IsVideo = true;
            }
            else
            {
                article_upd.IsVideo = false;
            }            
            _db.SaveChanges();
            return RedirectToAction("ArticleElement", "Admin", new { id = art_id });
        }
        [HttpPost]
        public async Task<IActionResult> UploadImageTMC(IFormFile file)
        {
            string checkImage;
            string checkDir;
            string image_directory;
            string image_directory_for_show;
            int tryChangeName = 0;
            string image_extension = Path.GetExtension(file.FileName); 
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = reader.ReadToEnd();
                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                
                checkImage = Path.GetFileNameWithoutExtension(file.FileName);
                checkDir = "./wwwroot/images/article_images/" + checkImage + image_extension;
                while (true)
                {
                    if(System.IO.File.Exists(checkDir) == true)
                    {
                        checkImage = checkImage + tryChangeName;
                        checkDir = "./wwwroot/images/article_images/" + checkImage + image_extension;
                        tryChangeName++;

                    }
                    else
                    {
                        break;
                    }
                    
                }

                string dirForShow = "/images/article_images/" + checkImage + image_extension;
                image_directory_for_show = dirForShow;
                image_directory = checkDir;
            }
            using(var stream = System.IO.File.Create(image_directory))
            {
                await file.CopyToAsync(stream);
            }
            return Json(new { location = image_directory_for_show });
        }
        public IActionResult ModuleAdd()
        {
            return View("ModuleAdd");
        }
        [HttpPost]
        public async Task<IActionResult> ModuleSave(string name, string descript)
        {
            Module module = new Module()
            {
                Name = name,
                Description = descript,
            };
            _db.Modules.Add(module);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }
    }
    
    }

