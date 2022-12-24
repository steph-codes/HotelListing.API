namespace HotelListing.API.Core.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object Key) : base($"{name} ({Key}) was not Found")
        {
            
        }
    }
}
