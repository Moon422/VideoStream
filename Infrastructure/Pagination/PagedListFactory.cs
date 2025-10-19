using System.Collections.Generic;
using VideoStream.Domain.Pagination;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure.Pagination;

public class PagedListFactory : IPagedListFactory
{
    public IPagedList<T> Create<T>(IList<T> items, int totalItems, int pageIndex, int pageSize)
        => new PagedList<T>(items, totalItems, pageIndex, pageSize);
}