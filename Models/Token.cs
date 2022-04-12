using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Models
{
    public class Token
    {
        public string AccesToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
