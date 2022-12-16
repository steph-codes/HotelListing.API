using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Country
{
    public class createCountryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
    }
}
