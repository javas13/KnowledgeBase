
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudyTodocups.DAL;

namespace StudyTodocups.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AdminAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public AdminAuthorizeAttribute()
        {
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ICurrentUser? currentUser = context.HttpContext.RequestServices.GetService<ICurrentUser>();

            bool isLoggedIn = currentUser.IsLoggedIn();
            if (isLoggedIn == false)
            {
                context.Result = new RedirectResult("/Login");
            }
            else if (currentUser.IsAdmin() == false)
            {
                context.Result = new RedirectResult("/Login");
            }


        }
        

    }
}