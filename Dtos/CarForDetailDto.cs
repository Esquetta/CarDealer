using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Entities;

namespace CarDealer.Dtos
{
    public class CarForDetailDto
    {
       
        public int Car_id { get; set; }
        [Display(Name ="Brand")]
        public string Brand { get; set; }
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Display(Name = "Horse Power")]
        public string Horse_Power { get; set; }
        [Display(Name = "Color")]
        public string Color { get; set; }
        [Display(Name = "Gearbox")]
        public string Gearbox { get; set; }
        [Display(Name = "Fuel Type")]
        public string Fuel_Type { get; set; }
        [Display(Name = "Miles")]
        public int Miles { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public bool For_Sale { get; set; }
        public List<Photo> Photos { get; set; }
        
    }
}
