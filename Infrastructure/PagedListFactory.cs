using System.Collections.Generic;
using VideoStream.Domain;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Infrastructure;

public class PagedListFactory : IPagedListFactory
{
    public IPagedList<T> Create<T>(IList<T> items, int totalItems, int pageIndex, int pageSize)
        => new PagedList<T>(items, totalItems, pageIndex, pageSize);
}