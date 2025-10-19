using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using VideoStream.Application.UseCases;
using VideoStream.Domain.Pagination;

namespace Presentation.Controllers;

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
    public async Task<ActionResult<PagedResponse<object>>> Search([FromQuery] string q, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(q)) return BadRequest("Query is required");
        var results = await _search.ExecuteAsync(q, page, pageSize);
        var response = new PagedResponse<object>
        {
            Items = results.Select(r => (object)r).ToList(),
            PageIndex = results.PageIndex,
            PageSize = results.PageSize,
            TotalItems = results.TotalItems,
            TotalPages = results.TotalPages
        };
        return Ok(response);
    }
}
