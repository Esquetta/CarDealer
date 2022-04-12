using CarDealer.Entities;
using System.Collections.Generic;

namespace CarDealer.Models
{
    public class CarInfoWithPageModel
    {
        public List<Car> cars { get; set; }
        public string CurrentModel { get; set; }
        public string Page { get; set; }
    }
}