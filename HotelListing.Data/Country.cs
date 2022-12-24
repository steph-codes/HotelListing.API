using System.Text.Json.Serialization;

namespace HotelListing.API.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        //many hotels one country
        public virtual IList<Hotel> Hotels { get; set; }
    }
}