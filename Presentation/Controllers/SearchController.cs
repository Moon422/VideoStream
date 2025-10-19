using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoStream.Application.UseCases;
using VideoStream.Presentation.Models;
using VideoStream.Presentation.Models.Search;

namespace VideoStream.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchVideosUseCase _search;

    public SearchController(SearchVideosUseCase search)
    {
        _search = search;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<SearchResult>>> Search([FromQuery] SearchCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Query)) return BadRequest("Query is required");
        var results = await _search.ExecuteAsync(command.Query, command.PageIndex, command.PageSize);
        var response = new PagedResponse<SearchResult>
        {
            Items = results.Select(r => new SearchResult
            {
                EntityType = r.Title,
                Id = r.Id,
                Title = r.Title,
                Description = r.Description
            }).ToList(),
            PageIndex = results.PageIndex,
            PageSize = results.PageSize,
            TotalItems = results.TotalItems,
            TotalPages = results.TotalPages
        };
        return Ok(response);
    }
}
