using ApiApplication.Domain.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.UseCases {
    public interface ICreateShowTimeUseCase {
        Task<ShowtimeEntity> Execute(int auditoriumId, string movieId, DateTime sessionDate, CancellationToken cancel);
    }
}