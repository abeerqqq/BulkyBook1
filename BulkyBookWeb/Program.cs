/*
 * Program.cs
 *  responsible for running our app, we use teh concept of DI here for example when we want to use DB / Email we define them here
 *  the service we want to add would be between builder and builder.Build()
 *  req pipline
 *      specefies how the app should respond to a web req
 *      How it works?
 *       the request from a browser goes through a pipline of middleware 
 *       MVC / Auth/ Authurization ... all are middleware 
 *       app.UseStaticFiles(); this is how we define a middleware
 *       Order is importnt!! first authenticate the user then authorize 
 *      
 *      
 */
using BulkyBook.DataAccess;
using Microsoft.AspNetCore.Identity;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
 * we add our DI here between builder and before we call build
 */

builder.Services.AddControllersWithViews(); // here we are adding a service to the container bc we are using MVC app
// If we are using Razor pages then the service would be diffrent
// Add DBContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*
 * app.use... is a middleware 
 */
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default", // the default if nothing provided 
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"); // action is the action of that controller
                                                                        //app.MapControllerRoute(
                                                                        //       name: "default",

//       pattern: "{area:Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
