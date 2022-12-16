using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Models
{
    public class GetCountryDto : BaseCountryDto
    {
        public int Id { get; set; }
       
    }
    
}
