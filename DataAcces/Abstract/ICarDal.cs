using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Entities;

namespace CarDealer.DataAcces
{
    public interface ICarDal:IRepository<Car>
    {
        List<Car> GetCarWithPhoto();

        Car GetCarById(int carId);
        List<Car> GetCarWithPhotoByName(string Carname);

        List<Car> GetSoldCars(string CarName);

       
    }
}
