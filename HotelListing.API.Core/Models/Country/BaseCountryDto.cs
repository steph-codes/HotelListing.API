using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Country
{
    //abstract class cant be instantiatied , can only be inherited
    public class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
