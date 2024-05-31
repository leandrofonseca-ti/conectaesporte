using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEsporte.Web.Areas.Professor.Controllers
{
	[Area("Professor")]
	public class CadastroHorarioController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
