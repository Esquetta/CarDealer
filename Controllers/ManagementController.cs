using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CarDealer.DataAcces;
using CarDealer.Dtos;
using CarDealer.Entities;
using CarDealer.Filter;
using CarDealer.Models;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rehper.Helpers;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore.Internal;
using CarDealer.Services;
using Microsoft.AspNetCore.Identity;
using CarDealer.AppIdentity;
using Microsoft.AspNetCore.Http;
using CarDealer.ExtensionMethods;
using CarDealer.Helpers.Abstract;

namespace CarDealer.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagementController : Controller
    {
        private IAuthRepository authRepository;
        private IConfiguration configuration;
        private IPhotoDal photoDal;
        private ICarDal carDal;
        private IMapper mapper;
        private IOptions<CloudinarySettings> cloudinaryOptions;
        private Cloudinary cloudinary;
        private CarDealerContext context;
        private ISessionService sessionService;
        private IQRCoder qrCoder;


        public ManagementController(IConfiguration configuration, IAuthRepository authRepository, ICarDal carDal, IMapper mapper, IOptions<CloudinarySettings> cloudinaryOptions, CarDealerContext context, IPhotoDal photoDal, ISessionService sessionService, IQRCoder qrCoder)
        {
            this.configuration = configuration;
            this.authRepository = authRepository;
            this.carDal = carDal;
            this.mapper = mapper;
            this.cloudinaryOptions = cloudinaryOptions;
            this.context = context;
            this.photoDal = photoDal;
            this.qrCoder = qrCoder;
            this.sessionService = sessionService;
            Account account = new Account(cloudinaryOptions.Value.CloudName, cloudinaryOptions.Value.ApiKey,
                cloudinaryOptions.Value.ApiSecret);
            cloudinary = new Cloudinary(account);

        }







        public IActionResult HomePage(int page = 1, string CarBrand = "")
        {
            int pageSize = 6;
            var cars = carDal.GetCarWithPhotoByName(CarBrand);
            var carsForReturn = mapper.Map<List<CarForListDto>>(cars);
            var model = new HomeModel
            {
                CarForListDto = carsForReturn.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageSize = pageSize,
                CurrentCar = CarBrand,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling(carsForReturn.Count / (double)pageSize),
            };

            return View(model);


        }
        //protected string GetName(string token)
        //{
        //    var key = Encoding.ASCII.GetBytes(configuration.GetSection("appsettings:Token").Value);
        //    var handler = new JwtSecurityTokenHandler();
        //    var validations = new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateIssuer = false,
        //        ValidateAudience = false
        //    };
        //    var claims = handler.ValidateToken(token, validations, out var decodedToken);
        //    return claims.Identity.Name;
        //}
        //public bool TokenCheck()
        //{
        //    var token = tokenSessionService.GetToken();
        //    if (token != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public IActionResult GetCarById(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = carDal.GetCarById(id);
            var carForReturn = mapper.Map<CarForDetailDto>(car);

            if (carForReturn == null)
            {
                return NotFound();
            }

            var url = $"https://localhost:44364/Car/detail?id={id}";
           var qrImage= qrCoder.GenerateCode(url);
            var model = new CarDetailListViewModel
            {
                CarForDetailDtos = carForReturn,
                QrImage=qrImage,
            };
            return View(model);

        }

        public IActionResult AddCar()
        {


            return View();

        }
        [HttpPost]
        public IActionResult AddCar(Car car)
        {
            if (car == null)
            {
                ViewBag.errorMessage = "Enter all textboxes";
                return View(car);
            }
            car.For_Sale = true;
            carDal.Add(car);
            ViewBag.SuccessMessage = "Car Created Successfully!";
            return View();
        }

        public IActionResult AddPhotoForCar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.unit = id;
            var model = new PhotoForCreationDto
            {
                CarId = id
            };

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPhotoForCar(PhotoForCreationDto photoForCreation)
        {
            var file = photoForCreation.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                foreach (var item in photoForCreation.File)
                {
                    using (var stream = item.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(item.Name, stream)
                        };
                        uploadResult = cloudinary.Upload(uploadParams);
                    }
                    photoForCreation.Url = uploadResult.Uri.ToString();
                    photoForCreation.PublicId = uploadResult.PublicId;
                    var photo = mapper.Map<Photo>(photoForCreation);
                    photo.IsMain = true;
                    photoDal.Add(photo);
                }
            }
            else
            {
                 TempData.Add("NoPhoto", "You must add photo you can upload it.");
                 return View();
            }


            //photo.Car = car;
            //if (!car.Photos.Any(m => m.IsMain))
            //{
            //    photo.IsMain = true;
            //}
            //photoDal.Add(photo);
            //if (context.SaveChanges() > 0)
            //{
            //    var photoToReturn = mapper.Map<PhotoForReturnDto>(photo);
            //    return RedirectToAction("HomePage", "Management");
            //}

            return RedirectToAction("Homepage", "Management");

        }

        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = carDal.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            carDal.Delete(car);
            return RedirectToAction("HomePage", "Management");
        }

        public IActionResult Update(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var updateCar = carDal.GetCarById(id);
            if (updateCar == null)
            {
                return NotFound();
            }


            return View(updateCar);
        }
        [HttpPost]
        public IActionResult Update(int id, Car car)
        {
            if (id != car.Car_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    carDal.Update(car);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return RedirectToAction("HomePage", "Management");
            }

            return View(car);
        }
        public IActionResult SoldCars(int page = 1, string CarBrand = "")
        {
            int pageSize = 6;
            var soldCars = carDal.GetSoldCars(CarBrand);
            var soldCarsReturn = mapper.Map<List<CarForListDto>>(soldCars);
            var model = new HomeModel
            {

                CarForListDto = soldCarsReturn.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                CurrentCar = CarBrand,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling(soldCarsReturn.Count / (double)pageSize),
                PageSize = pageSize


            };

            return View(model);
        }
        public IActionResult GetSoldCarById(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = carDal.GetCarById(id);
            var carForReturn = mapper.Map<CarForDetailDto>(car);

            if (carForReturn == null)
            {
                return NotFound();
            }

            var url = $"https://localhost:44364/Car/detail?id={id}";
            var qrImage = qrCoder.GenerateCode(url);
            var model = new CarDetailListViewModel
            {
                CarForDetailDtos = carForReturn,
                QrImage = qrImage,
            };
            return View(model);

        }

    }
}
