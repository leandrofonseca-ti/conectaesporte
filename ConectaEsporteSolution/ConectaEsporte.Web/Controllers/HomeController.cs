using ConectaEsporte.Web.Models;
using ConectaSolution.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ConectaSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ReturnEventPayment()
        {
            var responseString = Request.Form.ToString(); 
            Console.WriteLine(responseString);
            Response.WriteAsync(responseString);
            
            return View();
        }



        [HttpGet]
        public IActionResult ReturnEventPayment(int id)
        {
            var responseString = Request.Form != null ? Request.Form.ToString() : "";
            Console.WriteLine(responseString);
            Response.WriteAsync(responseString);

            return View();
        }
    }
}
