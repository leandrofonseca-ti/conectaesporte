using ConectaEsporte.Core.Services;
using ConectaEsporte.Core.Services.Repositories;
using ConectaEsporte.Web.Setup;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
var appSettings = builder.Configuration.GetSection("AppSettings");
var _domain = appSettings.GetValue<string>("Context").ToLower();

var _loginPath = _domain.Contains("professor") ? "/Access/LoginProfessor" : "/Access/LoginAluno";
// Add Signature
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option => {
		
		option.LoginPath = _loginPath;
		option.ExpireTimeSpan = TimeSpan.FromSeconds(30);
	});


builder.Services.ResolveDependenciesForCore(builder.Configuration);



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

app.UseRouting();

// Add Use Auth
app.UseAuthentication();

app.UseAuthorization();


if (_domain.Contains("professor"))
{
    app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Access}/{action=LoginProfessor}/{id?}");
    
}
else
{

    app.MapControllerRoute(
        name: "areaRoute",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    app.MapControllerRoute(
         name: "default",
         pattern: "{controller=Access}/{action=LoginAluno}/{id?}");
}
app.Run();
