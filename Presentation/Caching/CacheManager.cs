using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using VideoStream.Application.Caching;
using VideoStream.Application.Interfaces;
using VideoStream.Domain.Pagination;
using VideoStream.Infrastructure.Pagination;

namespace VideoStream.Presentation.Caching;

public class CacheManager : ICacheManager
{
    private readonly IMemoryCache
     _memoryCache;
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixes;

    private static readonly TimeSpan SlidingExpiration = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan AbsoluteExpiration = TimeSpan.FromHours(1);

    public CacheManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _prefixes = new ConcurrentDictionary<string, CancellationTokenSource>();
    }

    public CacheKey PrepareCacheKey(CacheKey key, params object[] parameters)
    {
        return new CacheKey(string.Format(key.Key, parameters), key.Prefix);
    }

    public async Task<T?> GetAsync<T>(CacheKey key, Func<Task<T?>> fetch)
    {
        if (_memoryCache.TryGetValue<T>(key.Key, out var result) && result is not null)
        {
            return result;
        }

        result = await fetch();
        if (result is null)
        {
            return result;
        }

        var options = new MemoryCacheEntryOptions
        {
            SlidingExpiration = SlidingExpiration,
            AbsoluteExpirationRelativeToNow = AbsoluteExpiration
        };

        if (string.IsNullOrWhiteSpace(key.Prefix))
        {
            return _memoryCache.Set(key.Key, result, options);
        }

        if (!_prefixes.TryGetValue(key.Prefix, out var cts) || cts is null)
        {
            cts = new CancellationTokenSource();
            _prefixes[key.Prefix] = cts;
        }

        options.AddExpirationToken(new CancellationChangeToken(cts.Token));
        return _memoryCache.Set(key.Key, result, options);
    }

    public async Task<IList<T>> GetAsync<T>(CacheKey key, Func<Task<IList<T>>> fetch)
    {
        return await _memoryCache.GetOrCreateAsync(key.Key, async entry =>
        {
            IList<T> result = await fetch();

            entry.SlidingExpiration = SlidingExpiration;
            entry.AbsoluteExpirationRelativeToNow = AbsoluteExpiration;

            if (string.IsNullOrWhiteSpace(key.Prefix))
            {
                return result;
            }

            if (_prefixes.TryGetValue(key.Prefix, out var cts) && cts is not null)
            {
                entry.AddExpirationToken(new CancellationChangeToken(cts.Token));
                return result;
            }

            cts = new CancellationTokenSource();
            entry.AddExpirationToken(new CancellationChangeToken(cts.Token));
            _prefixes[key.Prefix] = cts;
            return result;
        }) ?? [];
    }

    public async Task<IPagedList<T>> GetAsync<T>(CacheKey key, Func<Task<IPagedList<T>>> fetch)
    {
        return await _memoryCache.GetOrCreateAsync(key.Key, async entry =>
        {
            IPagedList<T> result = await fetch();

            entry.SlidingExpiration = SlidingExpiration;
            entry.AbsoluteExpirationRelativeToNow = AbsoluteExpiration;

            if (string.IsNullOrWhiteSpace(key.Prefix))
            {
                return result;
            }

            if (_prefixes.TryGetValue(key.Prefix, out var cts) && cts is not null)
            {
                entry.AddExpirationToken(new CancellationChangeToken(cts.Token));
                return result;
            }

            cts = new CancellationTokenSource();
            entry.AddExpirationToken(new CancellationChangeToken(cts.Token));
            _prefixes[key.Prefix] = cts;
            return result;
        }) ?? new PagedList<T>([], 0, 0, 1);
    }

    public async Task ClearCacheAsync()
    {
        foreach (var (_, cts) in _prefixes)
        {
            await cts.CancelAsync();
        }

        _prefixes.Clear();
    }

    public void RemoveCacheByKey(CacheKey key)
    {
        _memoryCache.Remove(key.Key);
    }

    public async Task RemoveCacheByPrefixAsync(string prefix)
    {
        if (_prefixes.TryGetValue(prefix, out var cts))
        {
            await cts.CancelAsync();
        }
    }
}