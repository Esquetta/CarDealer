using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarDealer.Entities;

namespace CarDealer.DataAcces
{
    public interface IRepository<T> where T :class, IEntity,new()
    {
        T Get(Expression<Func<T, bool>> filter = null);
        List<T> GetList(Expression<Func<T, bool>> filter = null);
        void Add(T entity);
        void Update(T entity);

        void Delete(T entity);

    }
}
