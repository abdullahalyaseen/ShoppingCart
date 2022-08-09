using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ShoppingCart.DataAccess.Interfaces;
using ShoppingCart.DataAccess;
using ShoppingCart.DataAccess.DbChangeTracking;
using ShoppingCart.Web.DbChangeTracking;
using ShoppingCart.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShoppingCart.Web;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ShoppingCartContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("ShoppingCart.DataAccess")));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDbTracker, DbTrack>();


builder.Services.AddIdentity<ApplicationUser, Role>().AddEntityFrameworkStores<ShoppingCartContext>();

builder.Services.Configure<IdentityOptions>(options =>
{

});

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/signin";
    config.LogoutPath = "/signout";
    config.AccessDeniedPath = "/accessdenied";
});




builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("view-management", policy => policy.RequireClaim("view", "management"));
    options.AddPolicy("view-categories", policy => policy.RequireClaim("view", "categories"));
    options.AddPolicy("add-category", policy => policy.RequireClaim("add", "category"));
    options.AddPolicy("edit-category", policy => policy.RequireClaim("edit", "category"));
    options.AddPolicy("delete-category", policy => policy.RequireClaim("delete", "category"));
    options.AddPolicy("view-users", policy => policy.RequireClaim("view", "users"));
    options.AddPolicy("add-user", policy => policy.RequireClaim("add", "user"));
    options.AddPolicy("edit-user", policy => policy.RequireClaim("edit", "user"));
    options.AddPolicy("delete-user", policy => policy.RequireClaim("delete", "user"));
    options.AddPolicy("view-products", policy => policy.RequireClaim("view", "products"));
    options.AddPolicy("add-product", policy => policy.RequireClaim("add", "product"));
    options.AddPolicy("edit-product", policy => policy.RequireClaim("edit", "product"));
    options.AddPolicy("delete-product", policy => policy.RequireClaim("delete", "product"));
    options.AddPolicy("view-tags", policy => policy.RequireClaim("view", "tags"));
    options.AddPolicy("add-tag", policy => policy.RequireClaim("add", "tag"));
    options.AddPolicy("edit-tag", policy => policy.RequireClaim("edit", "tag"));
    options.AddPolicy("delete-tag", policy => policy.RequireClaim("delete", "tag"));

});

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
app.UseStaticFiles(new StaticFileOptions{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
    RequestPath = new PathString("/vendor")
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Management",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();