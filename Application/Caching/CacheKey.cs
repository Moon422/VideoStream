namespace VideoStream.Application.Caching;

public class CacheKey
{
    public string Key { get; private set; }
    public string? Prefix { get; private set; }

    public CacheKey(string key, string? prefix = null)
    {
        Key = key;
        Prefix = prefix;
    }
}
