using Microsoft.AspNetCore.Authentication.OAuth;
using StudyTodocups.Data;
using StudyTodocups.Models;

namespace StudyTodocups.DAL
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext _db;
        public CurrentUser(IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
        {
            this.httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public bool IsLoggedIn()
        {
            int? id = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            return id != null;
        }
        public int GetUserId()
        {
            int? id = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            return (int)id;
        }
        public bool IsAdmin()
        {
            int? id = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            User usr = _db.Users.Where(i=> i.Id == id).FirstOrDefault();
            return usr.IsAdmin;
        }
    }
}
