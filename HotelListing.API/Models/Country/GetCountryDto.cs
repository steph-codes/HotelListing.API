using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Models
{
    //we woud have used this for update Dto since they're of the same ppties but 
    //we are using this for GET
    public class GetCountryDto : BaseCountryDto
    {
        public int Id { get; set; }
       
    }
    
}
