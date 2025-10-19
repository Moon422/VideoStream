namespace VideoStream.Presentation.Models.Search;

public record SearchResult : BaseModel
{
    public string EntityType { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}