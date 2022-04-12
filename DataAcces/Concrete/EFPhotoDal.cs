using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Entities;

namespace CarDealer.DataAcces
{
    public class EFPhotoDal : EfRepositoryBase<Photo, CarDealerContext>, IPhotoDal
    {
        public List<Photo> GetPhoto(int id)
        {
            using (var context=new CarDealerContext())
            {
                var result = context.Photos.Where(filter => filter.Id == id).ToList();
                return result;
            }
        }
    }
}
