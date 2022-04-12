using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Entities
{
    public class SoldCarOwner : IEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone_Number { get; set; }

        public string Adress { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
