using CarDealer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Services
{
    public interface ISessionService
    {
        void SetValue(string key);

        string GetValue(string key);

        void RemoveToken(string key);

        void  SetCart(Cart cart);

        void SetCarOwner(ShippingDetails shippingDetails);

        ShippingDetails GetCarOwner();
       

        Cart GetCart();

    }
}
