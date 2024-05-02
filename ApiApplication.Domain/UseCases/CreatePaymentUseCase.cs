using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.UseCases {
    internal class CreatePaymentUseCase : ICreatePaymentUseCase {
        private readonly ITicketsRepository _ticketsRepository;

        public CreatePaymentUseCase(ITicketsRepository ticketsRepository) {
            _ticketsRepository = ticketsRepository;
        }

        public async Task<TicketEntity> Execute(Guid reservationId, CancellationToken token = default) {

            TicketEntity ticket = await _ticketsRepository.GetAsync(reservationId, token).ConfigureAwait(false)
                ?? throw new EntityNotFoundException(reservationId.ToString(), nameof(TicketEntity));

            if (ticket.Paid) {
                throw PaymentException.TicketAlreadyPaid();
            }

            return ticket.IsExpired()
                ? throw PaymentException.ExpiredReservation()
                : await _ticketsRepository.ConfirmPaymentAsync(ticket, token);
        }
    }
}
