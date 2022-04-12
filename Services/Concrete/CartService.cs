using CarDealer.Dtos;
using CarDealer.Entities;
using CarDealer.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Services.Concrete
{
    public class CartService : ICartService
    {
        public void AddToCart(Cart cart,Car car)
        {
            
            cart.CartLines.Add(new CartLine { Car = car});
        }

        public List<CartLine> ListCart(Cart cart)
        {
            return cart.CartLines;
        }

        public void RemoveToCart(Cart cart, int id)
        {
            cart.CartLines.Remove(cart.CartLines.FirstOrDefault(filter=>filter.Car.Car_id==id));
        }
    }
}
