using AutoMapper;
using CarDealer.DataAcces;
using CarDealer.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.ViewComponents
{
    public class CarDetailListViewComponent : ViewComponent
    {
        private ICarDal carDal;
        private IMapper mapper;
        public CarDetailListViewComponent(ICarDal carDal, IMapper mapper)
        {
            this.carDal = carDal;
            this.mapper = mapper;
        }

        public ViewViewComponentResult Invoke(int id)
        {
            var car = carDal.GetCarById(id);
            var carForReturn = mapper.Map<CarForDetailDto>(car);

            var model = new CarDetailListViewModel
            {
                CarForDetailDtos = carForReturn
            };
            return View(model);

        }
    }
}
