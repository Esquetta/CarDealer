using CarDealer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CarDealer.Services
{
    public class SmtpService : ISmtpService
    {
        private SmtpClient smtpClient;
        public SmtpService(SmtpClient smtpClient)
        {
            this.smtpClient = smtpClient;
        }
        public void AccountConfirmMail(string ConfirmUrl, string UserName, string mailTo)
        {
            smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryFormat = SmtpDeliveryFormat.International;
            smtpClient.Credentials = new NetworkCredential("OtoFurki@hotmail.com", "Dolar11Tl!");

            MailAddress sender = new MailAddress("OtoFurki@hotmail.com", "CarDealer");
            MailAddress getter = new MailAddress(mailTo);
            MailMessage mailMessage = new MailMessage(sender, getter);
            mailMessage.Subject = "Account Activate Code";
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = $"Hi {UserName} here your activation code, {Environment.NewLine} https://localhost:44364" + ConfirmUrl;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
        }

        public void PasswordChanged(string UserName, string mailTo)
        {
            smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryFormat = SmtpDeliveryFormat.International;
            smtpClient.Credentials = new NetworkCredential("OtoFurki@hotmail.com", "Dolar11Tl!");

            MailAddress sender = new MailAddress("OtoFurki@hotmail.com", "CarDealer");
            MailAddress getter = new MailAddress(mailTo);
            MailMessage mailMessage = new MailMessage(sender, getter);
            mailMessage.Subject = "Your Password Changed";
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = $"Hi {UserName} your account password changed,<br> \n  if this was not you ,please immediately secure your account.Contat with us.";
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
        }

        public void PaymentReceipit(ShippingDetails shippingDetails, Cart cart)
        {
            smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("OtoFurki@hotmail.com", "Dolar11Tl!");
            MailAddress sender = new MailAddress("OtoFurki@hotmail.com", "CarDealer");
            MailAddress getter = new MailAddress(shippingDetails.Email);
            MailMessage mailMessage = new MailMessage(sender, getter);
            mailMessage.Subject = "Your payment receipit";
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = $"<b>Hi {shippingDetails.FirstName}</b> here your payment bill, " +
            $"<table><tr><td>" +
            $"CarName</td>" +
            $"<td>Model</td>" +
            $"<td>Color</td>" +
            $"<td>Price</td>" +
            $"</tr>" +
            $"<tr>" +
            $"<td>{cart.CartLines[0].Car.Brand}</td>" +
            $"<td>{cart.CartLines[0].Car.Model}</td>" +
            $"<td> {cart.CartLines[0].Car.Color} </td>" +
            $"<td> {cart.CartLines[0].Car.Price} </td>" +
            $"</tr></table>";
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);

        }

        public void ResetPasswordMail(string PasswordResetUrl, string UserName, string mailTo)
        {

            smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("OtoFurki@hotmail.com", "Dolar11Tl!");
            MailAddress Sender = new MailAddress("OtoFurki@hotmail.com", "Car Dealer");
            MailAddress Getter = new MailAddress(mailTo);
            MailMessage mailMessage = new MailMessage(Sender, Getter);
            mailMessage.Subject = "Password Reset";
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = $"Hi {UserName} here your password reset code, {Environment.NewLine} https://localhost:44364" + PasswordResetUrl;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
        }

        public void TwoFactorCodeMail(string username,string mailTo, string TwoFactorCode)
        {
            smtpClient = new SmtpClient("smtp.office365.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("OtoFurki@hotmail.com", "Dolar11Tl!");
            MailAddress Sender = new MailAddress("OtoFurki@hotmail.com", "Car Dealer");
            MailAddress Getter = new MailAddress(mailTo);
            MailMessage mailMessage = new MailMessage(Sender, Getter);
            mailMessage.Subject = "Two Factor Auth Code";
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = $"Hi {username} here your two factor authentication  code, {Environment.NewLine} " + TwoFactorCode;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
        }
    }
}
