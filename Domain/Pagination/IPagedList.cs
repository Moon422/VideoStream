using System.Collections.Generic;

namespace VideoStream.Domain.Pagination;

public interface IPagedList<T> : IList<T>
{
    public int PageIndex { get; set; }
    public int TotalItems { get; }
    public int TotalPages { get; }
    public int PageSize { get; set; }

    public int PageNumber { get; }
    public bool HasPrevious { get; }
    public bool HasNext { get; }
}
