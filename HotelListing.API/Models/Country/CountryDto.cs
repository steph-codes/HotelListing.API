﻿using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Country
{
    //This countryDto shows the full details including Hotels
    public class CountryDto
    {
        //full details of the country including Hotels
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public List<HotelDto> Hotels { get; set; }
    }
}
