using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyTodocups.DAL;
using StudyTodocups.Data;
using StudyTodocups.Models;

namespace StudyTodocups.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor httpContextAccessor;
        public LoginController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            this.httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View("Index", new User());
        }
        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> IndexSave(User usr2)
        {
            User usr = await _db.Users.Where(x => x.Login == usr2.Login && x.Password == usr2.Password).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                if (usr != null)
                {
                    httpContextAccessor.HttpContext?.Session.SetInt32("user_id", usr.Id);
                    if(usr.IsAdmin == false)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("Password", "Данные для входа неверны!");
                    return View("Index", usr2);
                }

            }

            else
            {
                return View("Index", usr2);
            }

        }
    }
}
