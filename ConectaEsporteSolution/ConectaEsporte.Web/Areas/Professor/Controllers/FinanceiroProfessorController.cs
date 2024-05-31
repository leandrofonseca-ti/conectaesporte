using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEsporte.Web.Areas.Professor.Controllers
{
	[Area("Professor")]
	public class FinanceiroProfessorController : Controller
    {
		[Authorize]
		public IActionResult Index()
        {
            return View();
        }
    }
}
