using System.Collections.Generic;
using System.Threading.Tasks;
using VideoStream.Application.DTOs;
using VideoStream.Domain;
using VideoStream.Domain.Entities;
using VideoStream.Domain.Interfaces;

namespace VideoStream.Application.UseCases;

public class SearchVideosUseCase
{
    private readonly ISearchService _searchService;
    private readonly IPagedListFactory _pagedListFactory;

    public SearchVideosUseCase(ISearchService searchService, IPagedListFactory pagedListFactory)
    {
        _searchService = searchService;
        _pagedListFactory = pagedListFactory;
    }

    public async Task<IPagedList<SearchResultDto>> ExecuteAsync(string query, int page = 0, int pageSize = 50)
    {
        var results = await _searchService.SearchAsync(query, page, pageSize);
        var mapped = new List<SearchResultDto>(results.Count);
        foreach (var item in results)
        {
            switch (item)
            {
                case Video v:
                    mapped.Add(new SearchResultDto
                    {
                        EntityType = SearchResultDtoDefaults.VideoEntityType,
                        Id = v.Id,
                        Title = v.Title,
                        Description = v.Description
                    });
                    break;
                case Channel c:
                    mapped.Add(new SearchResultDto
                    {
                        EntityType = SearchResultDtoDefaults.ChannelEntityType,
                        Id = c.Id,
                        Title = c.Name,
                        Description = c.Description
                    });
                    break;
            }
        }
        return _pagedListFactory.Create(mapped, results.TotalItems, results.PageIndex, results.PageSize);
    }
}
