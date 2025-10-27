namespace AppBookingTour.Share.DTOS;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    public bool HasPreviousPage => PageIndex > 0;
    public bool HasNextPage => PageIndex < TotalPages - 1;
}

public class PaginationRequest
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
}
