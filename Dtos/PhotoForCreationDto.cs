using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CarDealer.Dtos
{
    public class PhotoForCreationDto
    {
        public int CarId { get; set; }
        public string Url { get; set; }
        public IFormFile[] File { get; set; }
        public string PublicId { get; set; }
    }
}
