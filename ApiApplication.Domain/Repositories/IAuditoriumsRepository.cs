using ApiApplication.Domain.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.Repositories {
    public interface IAuditoriumsRepository {
        Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel);
    }
}