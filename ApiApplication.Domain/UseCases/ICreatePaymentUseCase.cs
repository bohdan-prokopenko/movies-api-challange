using ApiApplication.Domain.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.UseCases {
    public interface ICreatePaymentUseCase {
        Task<TicketEntity> Execute(Guid reservationId, CancellationToken token = default);
    }
}