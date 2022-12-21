using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users
{
    public  class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string lastName { get; set; }
        //APiUser inherits the rest of the fields for registgration from Loginto class
    }

}
