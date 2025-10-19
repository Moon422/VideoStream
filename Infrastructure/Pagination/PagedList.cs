using System;
using System.Collections.Generic;
using VideoStream.Domain;

namespace VideoStream.Infrastructure.Pagination;

public class PagedList<T> : List<T>, IPagedList<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }

    public int PageNumber => PageIndex + 1;
    public bool HasPrevious => PageIndex > 0;
    public bool HasNext => (PageIndex * PageSize + Count) < TotalItems;

    public PagedList(IList<T> source, int totalItems, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = Math.Max(pageSize, 1);
        TotalItems = totalItems;
        TotalPages = TotalItems / PageSize;
        if (TotalItems % PageSize > 0)
            TotalPages++;
        AddRange(source);
    }
}
