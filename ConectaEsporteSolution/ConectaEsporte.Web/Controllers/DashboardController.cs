using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ConectaEsporte.Core;
using ConectaEsporte.Core.Services.Repositories;
using ConectaEsporte.Web.Setup;
using System.Security.Claims;

namespace ConectaSolution.Controllers
{
 
	public class DashboardController : Controller
	{

		private readonly IUserRepository _userRepository;
		public DashboardController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}


		[Authorize]
		public IActionResult Index()
		{
			//ClaimsPrincipal claimUser = HttpContext.User;

			//if (claimUser.Identity.IsAuthenticated)
			//	return RedirectToAction("Index", "Dashboard");

			return View();
		}


		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			ConectaEsporte.Web.Setup.VariableHidden variables = new ConectaEsporte.Web.Setup.VariableHidden(this.HttpContext);

			if (variables.UserProfileId == EnumProfile.Aluno.GetHashCode())
			{
				return RedirectToAction("LoginAluno", "Access");
			}
			else
			{
				return RedirectToAction("LoginProfessor", "Access");
			}
		}



		public async Task<ActionResult> SetProfile(int userid, int profileid)
		{
			var result = await _userRepository.Login(userid, (EnumProfile)profileid);
			if (result != null && result.Id > 0)
			{
				ClaimsCookie cc = new ClaimsCookie(this.HttpContext.User, this.HttpContext);

				ClaimsCookie.KeyName[] keys = new[] {
					ClaimsCookie.KeyName.UserEmail,
					ClaimsCookie.KeyName.UserName,
					ClaimsCookie.KeyName.UserPicture,
					ClaimsCookie.KeyName.UserProfiles,
					ClaimsCookie.KeyName.UserProfileId,
					ClaimsCookie.KeyName.UserId,
				};

				String[] values = new[] {
					result.Email,
					result.Name,
					result.Picture,
					String.Join(",", result.Profiles.Select(y => y.Id)),
					profileid.ToString(),
					result.Id.ToString()};

				cc.SetValue(keys, values);


				ConfigurationBuilder builder = new ConfigurationBuilder();
				builder.AddJsonFile(System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json"), false, true);
				var configRoot = builder.Build();
				var rootUrl = configRoot.GetValue<string>("AppSettings:RootUrl");
				var url = string.Concat(rootUrl, "Dashboard/Index");
				return new JsonResult(new { ErrorMessage = "", Data = true, Redirect = url });
			}

			return new JsonResult(new { ErrorMessage = "", Data = false, Message = "Problemas ao atualizar login!" });

		}
	}
}
