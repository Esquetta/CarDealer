using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Controllers
{
    public class ExceptionController : Controller
    {
        public IActionResult Passwd()
        {
            return View();
        }
    }
}
