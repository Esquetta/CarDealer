using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Helpers.Abstract
{
    public interface IQRCoder
    {
        Byte[] GenerateCode(string url);
    }
}
