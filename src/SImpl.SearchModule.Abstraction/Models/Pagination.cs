namespace SImpl.SearchModule.Abstraction.Models
{
    public class Pagination
    {
        public long Total { get; set; }
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public int? TotalNumberOfPages { get; set; }
        public string SiteId { get; set; }
        public string BaseUrl { get; set; }
        public string QueryUrl { get; set; }
        public int ListingId { get; set; }
    }
}