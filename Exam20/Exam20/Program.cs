using Exam.Bussiness.Service.Abstract;
using Exam.Bussiness.Service.Concretes;
using Exam.Core.Models;
using Exam.Core.RepositoryAbstracts;
using Exam.Data.DAL;
using Exam.Data.RepositoryConcretes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer("Server=B3-2\\SQLEXPRESS01;Database=TeamTask;Trusted_Connection=true;Integrated Security=true;Encrypt=false");
});
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
    opt.Lockout.MaxFailedAccessAttempts = 3;
}
    ).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamRepository,TeamRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
