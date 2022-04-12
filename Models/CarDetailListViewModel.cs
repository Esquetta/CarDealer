using System.Collections;
using System.Collections.Generic;
using CarDealer.Dtos;

namespace CarDealer
{
    public class CarDetailListViewModel
    {
        public CarForDetailDto CarForDetailDtos { get; set; }
        public byte[] QrImage { get; set; }

    }
}