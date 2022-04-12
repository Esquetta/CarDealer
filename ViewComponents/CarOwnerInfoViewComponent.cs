using CarDealer.DataAcces.Abstract;
using CarDealer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.ViewComponents
{
    public class CarOwnerInfoViewComponent : ViewComponent
    {
        private ISoldCarOwnerDal carOwnerDal;
        public CarOwnerInfoViewComponent(ISoldCarOwnerDal carOwnerDal)
        {
            this.carOwnerDal = carOwnerDal;
        }
        public ViewViewComponentResult Invoke(int carId)
        {
            var CarOwner = carOwnerDal.Get(filter => filter.CarId == carId);
            var model = new SoldCarOwnerViewModel {
                SoldCarOwner = CarOwner
            };

            return View(model);


        }
    }
}
