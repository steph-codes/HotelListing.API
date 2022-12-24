namespace HotelListing.API.Core.Models
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }  //total count of the records clint can see 1000 records
        public int PageNumber { get; set; } // which page they're on
        public int RecordNumber { get; set; } //How many records per page
        public List<T> Items { get; set; }  //Actua; items returned
    }
}
