using System;

namespace VideoStream.Presentation.Models.Search;

public record SearchCommand
{
    public string Query { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int PageIndex => Math.Clamp(PageNumber, 1, int.MaxValue) - 1;
}
