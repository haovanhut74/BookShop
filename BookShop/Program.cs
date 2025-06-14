using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repository;
using BookShop.Repository.IRepository;
using BookShop.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();
// Nếu bạn có role và muốn dùng IdentityRole:
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// SỬA THỨ TỰ ĐĂNG KÝ DEPENDENCY INJECTION
builder.Services.AddScoped<ISP_Call, SP_Call>();
builder.Services.AddScoped<ICategoryReponsitory, CategoryReponsitory>();
builder.Services.AddScoped<ICoverTypeResponsitory, CoverTypeReponsitory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // UnitOfWork cuối cùng

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();