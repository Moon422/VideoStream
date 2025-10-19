using System.Collections.Generic;

namespace VideoStream.Presentation.Models;

public record PagedResponse<T> where T : BaseModel
{
    public IReadOnlyList<T> Items { get; set; } = [];
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
