using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Entities;

namespace CarDealer.DataAcces
{
    public class EFUserDal:EfRepositoryBase<User,CarDealerContext>,IUserDal
    {
    }
}
