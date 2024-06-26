﻿using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Repositories;
using ApiApplication.Repository.Context;

using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Repository.Database {
    internal class AuditoriumsRepository : IAuditoriumsRepository {
        private readonly CinemaContext _context;

        public AuditoriumsRepository(CinemaContext context) {
            _context = context;
        }

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel) {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .Include(x => x.Showtimes)
                .ThenInclude(x => x.Movie)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }
    }
}
