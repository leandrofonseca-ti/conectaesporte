using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEsporte.Web.Areas.Master.Controllers
{
	[Area("Master")]
	public class FinanceiroMasterController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
