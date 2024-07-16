using Microsoft.EntityFrameworkCore;
using StudyTodocups.Data;

namespace StudyTodocups
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(14400);
            });

            builder.Services.AddControllersWithViews();

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
            //"server=localhost;user=root;password=3022;database=study_todocups_db;",
            //new MySqlServerVersion(new Version(8, 0, 29))
            //));

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
            "server=localhost;user=root;password=lada5199824A!;database=study_todocups_db;",
            new MySqlServerVersion(new Version(5, 7, 42))
            ));

            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
            //"server=185.105.91.89;user=sammy2;password=lada5199824A!;database=study_todocups_db;",
            //new MySqlServerVersion(new Version(5, 7, 42))
            //));

            builder.Services.AddScoped<StudyTodocups.DAL.ICurrentUser, StudyTodocups.DAL.CurrentUser>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddMvc().AddSessionStateTempDataProvider();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}