using CarDealer.Entities;
using CarDealer.ExtensionMethods;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Services
{
    public class SessionService : ISessionService
    {
        private IHttpContextAccessor HttpContextAccessor;
        public SessionService(IHttpContextAccessor HttpContextAccessor)
        {
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public ShippingDetails GetCarOwner()
        {
            var value= HttpContextAccessor.HttpContext.Session.GetObject<ShippingDetails>("carowner");
            if (value==null)
            {
                return null;
            }
            return value;
        }

        public Cart GetCart()
        {
            Cart cartCheck = HttpContextAccessor.HttpContext.Session.GetObject<Cart>("cart");
            if (cartCheck == null)
            {
                HttpContextAccessor.HttpContext.Session.SetObject("cart", new Cart());
                cartCheck = HttpContextAccessor.HttpContext.Session.GetObject<Cart>("cart");
            }
            return cartCheck;
        }

      

        public string GetValue(string key)
        {
            var value = HttpContextAccessor.HttpContext.Session.GetObject<string>(key);
            if (value == null)
            {
                return null;
            }
            return value;
        }

        

        public void RemoveToken(string key)
        {
            HttpContextAccessor.HttpContext.Session.Remove(key);
        }

        public void SetCarOwner(ShippingDetails shippingDetails)
        {
            HttpContextAccessor.HttpContext.Session.SetObject("carowner", shippingDetails);
        }

        public void SetCart(Cart cart)
        {
            HttpContextAccessor.HttpContext.Session.SetObject("cart",cart);
        }

        public void SetValue(string key)
        {
            HttpContextAccessor.HttpContext.Session.SetObject("key",key);
        }
    }
}
