using ApiApplication.Domain.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.Api {
    public interface IMoviesClientApi {
        Task<MovieEntity> GetById(string movieId, CancellationToken token = default);
    }
}
