using Microsoft.AspNetCore.Mvc;

namespace ConectaEsporteSolution
{
	public class AccessController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
