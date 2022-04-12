using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Dtos
{
    public class CarForListDto
    {
        
        public int Car_id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Age { get; set; }
        public decimal Price { get; set; }
        public string Horse_Power { get; set; }
        public string Color { get; set; }
        public string Gearbox { get; set; }
        public string Fuel_Type { get; set; }
        public int Miles { get; set; }
        public string Description { get; set; }
        public bool For_Sale { get; set; }
        public string PhotoUrl { get; set; }
    }
}
