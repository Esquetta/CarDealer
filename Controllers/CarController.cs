using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarDealer.DataAcces;
using CarDealer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.Extensions.Localization;
using CarDealer.Resources;

namespace CarDealer.Controllers
{

    [Route("Car")]
    public class CarController : Controller
    {
        private ICarDal carDal;
        private IPhotoDal photoDal;
        private IMapper mapper;
        public CarController(ICarDal carDal, IMapper mapper, IPhotoDal photoDal)
        {
            this.carDal = carDal;
            this.mapper = mapper;
            this.photoDal = photoDal;
        }

        public IActionResult Index(int page = 1, string CarBrand = "")
        {

            int pageSize = 6;
            var cars = carDal.GetCarWithPhotoByName(CarBrand);
            var carsForReturn = mapper.Map<List<CarForListDto>>(cars);
           
            var model = new CarsListViewModel
            {
                CarForListDtos = carsForReturn.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageCount = (int)Math.Ceiling(cars.Count / (double)pageSize),
                CurrentCar = CarBrand,
                CurrentPage = page,
                PageSize = pageSize


            };

            return View(model);
        }
        
        [HttpGet]
        [Route("detail")]
        public IActionResult GetCarById(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = carDal.GetCarById(id);
            var carForReturn = mapper.Map<CarForDetailDto>(car);

            if (carForReturn == null)
            {
                return NotFound();
            }


            var model = new CarDetailListViewModel
            {
                CarForDetailDtos = carForReturn
            };
            return View(model);

        }

        [HttpGet]
        [Route("Photos")]
        public ActionResult GetPhotoById(int id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Car not found");
            }
            var photos = photoDal.GetPhoto(id);
            return Ok(photos);
        }
        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

    }
}
