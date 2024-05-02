using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.UseCases {
    internal sealed class CreateReservationUseCase : ICreateReservationUseCase {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly ITicketsRepository _ticketsRepository;

        public CreateReservationUseCase(IAuditoriumsRepository auditoriumsRepository, ITicketsRepository ticketsRepository, IShowtimesRepository showtimesRepository) {
            _auditoriumsRepository = auditoriumsRepository;
            _ticketsRepository = ticketsRepository;
            _showtimesRepository = showtimesRepository;
        }

        public async Task<TicketEntity> Execute(int showtimeId, short row, IEnumerable<short> requestedSeatNumbers, CancellationToken token = default) {
            ShowtimeEntity showtime = await _showtimesRepository.GetWithTicketsByIdAsync(showtimeId, token).ConfigureAwait(false)
                ?? throw new EntityNotFoundException(showtimeId, nameof(ShowtimeEntity));

            AuditoriumEntity auditorium = await _auditoriumsRepository.GetAsync(showtime.AuditoriumId, token)
                ?? throw new EntityNotFoundException(showtime.AuditoriumId, nameof(AuditoriumEntity));

            IEnumerable<SeatEntity> requestedSeats = auditorium.Seats
                .Where(s => s.Row.Equals(row))
                .Where(s => requestedSeatNumbers.Contains(s.SeatNumber))
                .ToHashSet();

            if (!requestedSeats.Any()) {
                throw new EntityNotFoundException(showtimeId, nameof(SeatEntity));
            }

            if (!IsContiguous(requestedSeatNumbers)) {
                throw ReservationException.SeatsAreNotContiguous(requestedSeatNumbers);
            }

            IEnumerable<TicketEntity> showtimeTickets = await _ticketsRepository.GetEnrichedAsync(showtimeId, token) ?? Enumerable.Empty<TicketEntity>();
            IEnumerable<SeatEntity> reservedSeats = showtimeTickets
                .Where(t => t.Paid || !t.IsExpired())
                .SelectMany(t => t.Seats)
                .Where(s => requestedSeats.Contains(s));

            var isRequestedSeatsAlreadyReserved = reservedSeats.Any();

            return isRequestedSeatsAlreadyReserved
                ? throw ReservationException.SeatsAreAlredyReserved(reservedSeats.Select(s => s.SeatNumber))
                : await _ticketsRepository.CreateAsync(showtime, requestedSeats, token);
        }

        private bool IsContiguous(IEnumerable<short> numbers) {
            var array = numbers.ToArray();
            Array.Sort(array);

            for (var i = 0; i < array.Length - 1; i++) {
                if (array[i] + 1 != array[i + 1]) {
                    return false;
                }
            }

            return true;
        }
    }
}
