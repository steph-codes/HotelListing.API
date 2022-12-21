﻿using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;
using HotelListing.API.Models.Users;

namespace HotelListing.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //reverse maps maps it from createcountrydto to country
            CreateMap<Country, createCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, updateCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();

            //HotelDto
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<ApiUserDto, ApiUser> ().ReverseMap();
        }
    }
}
