using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Core.Models.Hotel;
using HotelListing.API.Core.Models.Users;

namespace HotelListing.API.Core.Configurations
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
