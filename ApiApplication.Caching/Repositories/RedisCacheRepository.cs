using ApiApplication.Domain.Repositories;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using static System.TimeSpan;

namespace ApiApplication.Caching.Repositories {
    internal class RedisCacheRepository : ICacheRepository {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheRepository> _logger;

        public RedisCacheRepository(IDistributedCache cache, ILogger<RedisCacheRepository> logger) {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetObjectFromCache<T>(string key, CancellationToken token) {
            var cachedData = await _cache.GetAsync(key, token);

            if (cachedData != null) {
                using var memoryStream = new MemoryStream(cachedData);
                var serializer = new BinaryFormatter();
                return (T)serializer.Deserialize(memoryStream);
            }

            return default;
        }

        public async Task SetObjectInCache<T>(string key, T value, int minutes = 0, CancellationToken token = default) {
            try {
                using var memoryStream = new MemoryStream();
                var serializer = new BinaryFormatter();
                serializer.Serialize(memoryStream, value);
                var dataToCache = memoryStream.ToArray();

                var options = new DistributedCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = FromMinutes(minutes),
                };

                await _cache.SetAsync(key, dataToCache, options, token);
            } catch (Exception ex) {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
