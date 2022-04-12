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
    public class PhotosViewComponent : ViewComponent
    {
        private IPhotoDal photoDal;
        public PhotosViewComponent(IPhotoDal photoDal)
        {
            this.photoDal = photoDal;
        }

        public ViewViewComponentResult Invoke()
        {
            var model = new PhotosListViewModel
            {

                Photos = photoDal.GetList()
            };
            return View(model);
        }
    }
}
