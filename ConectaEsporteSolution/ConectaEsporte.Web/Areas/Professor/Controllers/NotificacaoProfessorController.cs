using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEsporte.Web.Areas.Professor.Controllers
{
	[Area("Professor")]
	public class NotificacaoProfessorController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
