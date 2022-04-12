using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Entities;

namespace CarDealer.DataAcces
{
    public interface IPhotoDal:IRepository<Photo>
    {
        List<Photo> GetPhoto(int id);
    }
}
