using SimpleWebApp.DTOs.Page;

namespace SimpleWebApp.DTOs.Filter
{
    public class FilterRequestDto<TFilters>
    {
        public TFilters Filter { get; set; }
        public PagedRequestDto Pagination { get; set; }
        public IEnumerable<string> SortRules { get; set; }
    }
}
