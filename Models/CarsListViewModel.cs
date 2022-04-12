using System.Collections.Generic;
using CarDealer.Dtos;

namespace CarDealer
{
    public class CarsListViewModel
    {
        public List<CarForListDto> CarForListDtos { get; set; }
        public int PageCount { get; set; }
        public string CurrentCar { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}