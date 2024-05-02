using ApiApplication.Domain.Entities;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.UseCases {
    public interface ICreateReservationUseCase {
        Task<TicketEntity> Execute(int showtimeId, short row, IEnumerable<short> seats, CancellationToken token = default);
    }
}