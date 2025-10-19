using VideoStream.Domain.Entities;

namespace VideoStream.Application.Caching;

public static partial class EntityCacheDefaults<T> where T : BaseEntity
{
    public static string EntityTypeName => typeof(T).Name.ToLowerInvariant();

    public static CacheKey ByIdCacheKey => new($"Lunafy.{EntityTypeName}.byid.{{0}}", ByIdPrefix);

    public static CacheKey ByIdsCacheKey => new($"Lunafy.{EntityTypeName}.byids.{{0}}", ByIdsPrefix);

    public static CacheKey AllCacheKey => new($"Lunafy.{EntityTypeName}.all", AllPrefix);

    public static CacheKey AllPagedCacheKey => new($"Lunafy.{EntityTypeName}.all.paged.{{0}}.{{1}}", AllPagedPrefix);

    public static string Prefix => $"Lunafy.{EntityTypeName}.";

    public static string ByIdPrefix => $"Lunafy.{EntityTypeName}.byid.";

    public static string ByIdsPrefix => $"Lunafy.{EntityTypeName}.byids.";

    public static string AllPrefix => $"Lunafy.{EntityTypeName}.all";

    public static string AllPagedPrefix => $"Lunafy.{EntityTypeName}.all.paged.";
}