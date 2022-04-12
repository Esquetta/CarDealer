using CarDealer.Dtos;
using CarDealer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Services.Abstract
{
    public interface ICartService
    {
        List<CartLine> ListCart(Cart cart);
        void AddToCart(Cart cart,Car car);
        void RemoveToCart(Cart cart, int id);
    }
}
