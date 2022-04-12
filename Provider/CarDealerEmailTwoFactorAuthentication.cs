using CarDealer.AppIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Provider
{
    public class CarDealerEmailTwoFactorAuthentication<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : AppIdentityUser
    {
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            if (manager != null && user != null)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(GeneraterToken(user,purpose));
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(token==GeneraterToken(user,purpose));
        }
        private string GeneraterToken(AppIdentityUser user, string purpose)
        {
            Random random = new Random();
            var superSecretToken =random.Next(1000,9999);
            return $"{superSecretToken}";
        }
    }
}
