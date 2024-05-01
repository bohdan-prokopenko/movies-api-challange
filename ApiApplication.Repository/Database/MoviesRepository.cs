using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Repositories;
using ApiApplication.Repository.Context;

using Microsoft.EntityFrameworkCore;

using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Repository.Database {
    internal class MoviesRepository : IMoviesRepository {
        private readonly CinemaContext _context;

        public MoviesRepository(CinemaContext context) {
            _context = context;
        }

        public async Task<MovieEntity> CreateAsync(MovieEntity movie, CancellationToken cancel = default) {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<MovieEntity> entry = _context.Movies.Add(movie);

            _ = await _context.SaveChangesAsync(cancel);

            return entry.Entity;
        }

        public async Task<MovieEntity> GetByExternalIdAsync(string externalId, CancellationToken token = default) {
            return await _context.Movies.FirstOrDefaultAsync(m => m.ExternalId.Equals(externalId), token);
        }
    }
}
