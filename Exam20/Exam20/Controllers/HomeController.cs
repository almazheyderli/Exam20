using Exam20.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exam20.Controllers
{
    public class HomeController : Controller
    {
      

        public IActionResult Index()
        {
            return View();
        }

   
    }
}