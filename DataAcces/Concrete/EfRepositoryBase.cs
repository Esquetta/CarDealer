using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CarDealer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.DataAcces
{
    public class EfRepositoryBase<Tentity, Tcontext> : IRepository<Tentity> where Tentity : class, IEntity, new()
    where Tcontext : DbContext, new()
    {
        public void Add(Tentity entity)
        {
            using (var context = new Tcontext())
            {
                var addEntity = context.Add(entity);
                addEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(Tentity entity)
        {
            using (var context = new Tcontext())
            {
                var deleteContext = context.Remove(entity);
                deleteContext.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public Tentity Get(Expression<Func<Tentity, bool>> filter = null)
        {
            using (var context = new Tcontext())
            {
                return context.Set<Tentity>().SingleOrDefault(filter);
            }
        }

        public List<Tentity> GetList(Expression<Func<Tentity, bool>> filter = null)
        {
            using (var context = new Tcontext())
            {
                return filter == null ? context.Set<Tentity>().ToList() : context.Set<Tentity>().Where(filter).ToList();
            }
        }

        public void Update(Tentity entity)
        {
            using (var context = new Tcontext())
            {
                var updateContext = context.Update(entity);
                updateContext.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
