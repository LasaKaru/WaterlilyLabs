using Microsoft.Extensions.Caching.Memory;

namespace WaterlilyLabs.Services
{
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache _memoryCache;

        public CachingService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? Get<T>(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out T? value);
            return value;
        }

        public void Set<T>(string cacheKey, T value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, bool neverRemove = false)
        {
            var options = new MemoryCacheEntryOptions();

            if (neverRemove)
            {
                options.Priority = CacheItemPriority.NeverRemove;
            }
            else
            {
                if (absoluteExpirationRelativeToNow.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
                }
                if (slidingExpiration.HasValue)
                {
                    options.SlidingExpiration = slidingExpiration;
                }
            }
            _memoryCache.Set(cacheKey, value, options);
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        // Requirement: Cached: Cache a generic result up to 5 minutes
        public async Task<T> CachedAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, TimeSpan duration)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out T? cacheEntry))
            {
                cacheEntry = await getItemCallback();
                if (cacheEntry != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(duration);
                    _memoryCache.Set(cacheKey, cacheEntry, cacheEntryOptions);
                }
            }
            return cacheEntry!; 
        }

        // Requirement: CachedLong: Cache a generic result until cache is removed
        public async Task<T> CachedLongAsync<T>(string cacheKey, Func<Task<T>> getItemCallback)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out T? cacheEntry))
            {
                cacheEntry = await getItemCallback();
                if (cacheEntry != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                         // .SetPriority(CacheItemPriority.NeverRemove);
                         .SetAbsoluteExpiration(TimeSpan.FromDays(365)); 
                    _memoryCache.Set(cacheKey, cacheEntry, cacheEntryOptions);
                }
            }
            return cacheEntry!;
        }
    }
}
