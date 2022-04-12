using CarDealer.DataAcces.Abstract;
using CarDealer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.DataAcces.Concrete
{
    public class EFActivityLogDal:EfRepositoryBase<ActivityLog,CarDealerContext>, IActivityLogDal
    {
    }
}
