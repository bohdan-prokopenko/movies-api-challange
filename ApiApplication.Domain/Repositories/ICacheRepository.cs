using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.Repositories {
    public interface ICacheRepository {
        Task<T> GetObjectFromCache<T>(string key, CancellationToken token = default);
        Task SetObjectInCache<T>(string key, T value, int lifetimeInMinutes = 0, CancellationToken token = default);
    }
}
