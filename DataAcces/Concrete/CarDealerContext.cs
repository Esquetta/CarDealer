using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.DataAcces
{
    public class CarDealerContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<SoldCarOwner> SoldCarOwners { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=DESKTOP-EMPIGDT;Initial Catalog=CarGallery;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
