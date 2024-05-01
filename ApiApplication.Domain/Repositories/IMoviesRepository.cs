using ApiApplication.Domain.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.Repositories {
    public interface IMoviesRepository {
        Task<MovieEntity> CreateAsync(MovieEntity movie, CancellationToken token = default);
        Task<MovieEntity> GetByExternalIdAsync(string externalId, CancellationToken token = default);
    }
}