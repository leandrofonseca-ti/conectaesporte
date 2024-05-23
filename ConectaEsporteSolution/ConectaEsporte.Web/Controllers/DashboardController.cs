using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ConectaEsporte.Core;

namespace ConectaSolution.Controllers
{
	[Authorize]
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
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
	}
}
