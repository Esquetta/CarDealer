using CarDealer.DataAcces;
using CarDealer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.ViewComponents
{
    public class CarBrandListViewComponent : ViewComponent
    {
        private ICarDal cardal;
        public CarBrandListViewComponent(ICarDal cardal)
        {
            this.cardal = cardal;
        }
        public ViewViewComponentResult Invoke(string Page)
        {
            var cars = cardal.GetList();

            var model = new CarInfoWithPageModel
            {
                cars = cars,
                CurrentModel = HttpContext.Request.Query["CarBrand"],
                Page = Page
            };
            return View(model);

        }
    }
}
