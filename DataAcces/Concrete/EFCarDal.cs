using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.DataAcces
{
    public class EFCarDal : EfRepositoryBase<Car, CarDealerContext>, ICarDal
    {
        public Car GetCarById(int carId)
        {
            using (var context = new CarDealerContext())
            {
                var result = context.Cars.Include(p => p.Photos).FirstOrDefault(filter => filter.Car_id == carId);
                return result;
            }
        }

        public List<Car> GetCarWithPhoto()
        {
            using (var context = new CarDealerContext())
            {
                var result = context.Cars.Include(p => p.Photos).ToList();
                return result;
            }
        }
        public List<Car> GetCarWithPhotoByName(string CarName)
        {
            using (var context = new CarDealerContext())
            {
                if (!String.IsNullOrEmpty(CarName))
                {
                    var result = context.Cars.Include(p => p.Photos).Where(n => n.Brand == CarName || CarName == "" && n.For_Sale == true).ToList();
                    return result;
                }
                var All = context.Cars.Include(p => p.Photos).Where(filter => filter.For_Sale == true).ToList();
                return All;
            }
        }

        public List<Car> GetSoldCars(string CarName)
        {
            using (var context=new CarDealerContext())
            {
                if (!string.IsNullOrEmpty(CarName))
                {
                    return context.Cars.Include(p=>p.Photos).Where(filter => filter.For_Sale == false && filter.Brand==CarName).ToList();
                }
                return context.Cars.Include(p => p.Photos).Where(filter => filter.For_Sale == false).ToList();
            }
        }
    }
}
