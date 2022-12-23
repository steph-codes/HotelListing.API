using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string lastName { get; set; }
    }
}
