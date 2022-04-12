using AutoMapper;
using CarDealer.DataAcces;
using CarDealer.Dtos;
using CarDealer.Services;
using CarDealer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using CarDealer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealer.Models;
using CarDealer.DataAcces.Abstract;

namespace CarDealer.Controllers
{
    public class PaymentController : Controller
    {
        private ICarDal carDal;
        private IMapper mapper;
        private ISessionService sessionService;
        private ICartService cartService;
        private ISmtpService smtpService;
        private ISoldCarOwnerDal soldCarOwnerDal;
        public PaymentController(ICarDal carDal, IMapper mapper, ISessionService sessionService, ICartService cartService, ISmtpService smtpService, ISoldCarOwnerDal soldCarOwnerDal)
        {
            this.carDal = carDal;
            this.mapper = mapper;
            this.sessionService = sessionService;
            this.cartService = cartService;
            this.smtpService = smtpService;
            this.soldCarOwnerDal = soldCarOwnerDal;
        }

        public IActionResult BuyCar(int id)
        {
            var forSaleCar = carDal.Get(filter => filter.Car_id == id);
            var cart = sessionService.GetCart();
            cartService.AddToCart(cart, forSaleCar);
            sessionService.SetCart(cart);

            return RedirectToAction("UserPaymentInfo", "Payment");
        }

        public IActionResult UserPaymentInfo()
        {
            var model = new ShippingDetailsViewModel
            {
                ShippingDetails = new ShippingDetails()
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult UserPaymentInfo(ShippingDetailsViewModel shippingDetailsViewModel)
        {
            var cart = sessionService.GetCart();
            if (ModelState.IsValid)
            {

                var owner = sessionService.GetCarOwner();
                sessionService.SetCarOwner(shippingDetailsViewModel.ShippingDetails);
                return RedirectToAction("BankPayment");
            }
            return View();
        }
        public IActionResult BankPayment()
        {
            if (true)/* Eğer banka api'si ile ödeme başarılı olursa*/
            {
                var buyer = sessionService.GetCarOwner();
                var selledCar = sessionService.GetCart();
                selledCar.CartLines[0].Car.For_Sale = false;
                carDal.Update(selledCar.CartLines[0].Car);
                var CarOwner = mapper.Map<SoldCarOwner>(buyer);
                CarOwner.CarId = selledCar.CartLines[0].Car.Car_id;
                soldCarOwnerDal.Add(CarOwner);
                smtpService.PaymentReceipit(buyer, selledCar);

            }
            return View();
        }
    }
}
