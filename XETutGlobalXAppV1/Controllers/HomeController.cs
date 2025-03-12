using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using XETutGlobalXAppV1.Models;

namespace XETutGlobalXAppV1.Controllers
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
        public IActionResult HelpDesk()
        {
            return View();
        }
        public IActionResult InstructorPortal()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitHelpDesk(string formdata)
        {
            string data = formdata;

            XEtutGlobalX.Modal.Material.HelpDeskForm.Root root = new();
            if(data!=null)
            {
                root = JsonConvert.DeserializeObject<XEtutGlobalX.Modal.Material.HelpDeskForm.Root>(data)!;
            }
            else
            {
                root = null!;
            }
            if(root!=null)
            {
                
            }
            return Json(new
            {
               isSuccess = true,
               
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
