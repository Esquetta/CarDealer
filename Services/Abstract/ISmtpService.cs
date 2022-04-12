using CarDealer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CarDealer.Services
{
    public interface ISmtpService
    {
        void ResetPasswordMail(string PasswordResetUrl, string UserName, string mailTo);
        void AccountConfirmMail(string ConfirmUrl, string UserName,string mailTo);
        void PaymentReceipit(ShippingDetails shippingDetails,Cart cart);
        void PasswordChanged(string UserName, string mailTo);
        void TwoFactorCodeMail(string username,string mailTo,string TwoFactorCode);
    }
}
