using System.Collections.Generic;

namespace VideoStream.Domain.Interfaces;

public interface IPagedListFactory
{
    IPagedList<T> Create<T>(IList<T> items, int totalItems, int pageIndex, int pageSize);
}