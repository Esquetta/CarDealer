using CarDealer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarDealer.Entities
{
    public class CartLine
    {
        
        public Car Car { get; set; }
    }
}
