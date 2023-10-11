namespace SimpleWebApp.DTOs.Page
{
    public class PagedResponseDto<TData>
    {
        public IEnumerable<TData> Items { get; set; }
        public int Limit { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
