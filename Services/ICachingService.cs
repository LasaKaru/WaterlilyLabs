namespace WaterlilyLabs.Services
{
    public interface ICachingService
    {
        T? Get<T>(string cacheKey);
        void Set<T>(string cacheKey, T value, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, bool neverRemove = false);
        void Remove(string cacheKey);

        Task<T> CachedAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, TimeSpan duration);
        Task<T> CachedLongAsync<T>(string cacheKey, Func<Task<T>> getItemCallback);
    }
}
