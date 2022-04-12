using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Models;

namespace CarDealer.ViewComponents
{
    public class AdminWelcomeViewComponent : ViewComponent
    {
        public ViewViewComponentResult Invoke()
        {

            var model = new UserInfoViewModel
            {
                username = HttpContext.User.Identity.Name
            };

            return View(model);
        }

    }
}

