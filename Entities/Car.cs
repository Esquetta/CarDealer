using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Entities
{
    public class Car : IEntity
    {
        public Car()
        {
            Photos = new List<Photo>();
        }
        [Key]
        [Required]
        public int Car_id { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Horse Power")]
        public string Horse_Power { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Gearbox { get; set; }
        [Required]
        [Display(Name ="Fuel Type")]
        public string Fuel_Type { get; set; }
        [Required]
        public int Miles { get; set; } 
        [Required]
        public bool For_Sale { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<Photo> Photos { get; set; }
    }
}
