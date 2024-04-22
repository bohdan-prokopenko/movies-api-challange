using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories {
    public class TicketsRepository : ITicketsRepository {
        private readonly CinemaContext _context;

        public TicketsRepository(CinemaContext context) {
            _context = context;
        }

        public Task<TicketEntity> GetAsync(Guid id, CancellationToken cancel) {
            return _context.Tickets.FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public async Task<IEnumerable<TicketEntity>> GetEnrichedAsync(int showtimeId, CancellationToken cancel) {
            return await _context.Tickets
                .Include(x => x.Showtime)
                .Include(x => x.Seats)
                .Where(x => x.ShowtimeId == showtimeId)
                .ToListAsync(cancel);
        }

        public async Task<TicketEntity> CreateAsync(ShowtimeEntity showtime, IEnumerable<SeatEntity> selectedSeats, CancellationToken cancel) {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TicketEntity> ticket = _context.Tickets.Add(new TicketEntity {
                Showtime = showtime,
                Seats = new List<SeatEntity>(selectedSeats)
            });

            _ = await _context.SaveChangesAsync(cancel);

            return ticket.Entity;
        }

        public async Task<TicketEntity> ConfirmPaymentAsync(TicketEntity ticket, CancellationToken cancel) {
            ticket.Paid = true;
            _ = _context.Update(ticket);
            _ = await _context.SaveChangesAsync(cancel);
            return ticket;
        }
    }
}
