using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CarDealer.Dtos;
using CarDealer.Entities;

namespace CarDealer.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Car, CarForListDto>().ForMember(dest => dest.PhotoUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(filter => filter.IsMain).Url);
            });
            CreateMap<Car, CarForDetailDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<PhotoForReturnDto, Photo>();
            CreateMap<ShippingDetails,SoldCarOwner>();
            
        }
    }
}
