using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;

namespace HotelListing.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //reverse maps maps it from createcountrydto to country
            CreateMap<Country, CreateCountryDto>().ReverseMap();
        }
    }
}
