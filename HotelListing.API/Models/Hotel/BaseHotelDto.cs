using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Hotel
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        //? adds nullable or accept nothing so number field doesnt enforce 0
        public double? Rating { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        //adding a valid foreign key
        public int CountryId { get; set; }
    }
}
