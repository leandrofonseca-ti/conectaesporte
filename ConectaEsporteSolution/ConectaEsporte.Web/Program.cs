using ConectaEsporte.Core.Services;
using ConectaEsporte.Core.Services.Repositories;
using ConectaEsporte.Web.Setup;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Signature
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option => {
		option.LoginPath = "/Access/LoginAluno";
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


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Access}/{action=LoginAluno}/{id?}");

app.Run();
