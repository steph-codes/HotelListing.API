namespace HotelListing.API.Core.Models.Hotel
{
    public class HotelDto : BaseHotelDto
    {
        // for Update Hotel Dto you use the Get hotel since its the same object
        public int Id { get; set; }
       
    }
}
