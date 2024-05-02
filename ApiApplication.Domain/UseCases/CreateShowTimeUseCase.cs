using ApiApplication.Domain.Api;
using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Repositories;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.UseCases {
    internal class CreateShowTimeUseCase : ICreateShowTimeUseCase {
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IMoviesClientApi _moviesApi;
        private readonly IMoviesRepository _moviesRepository;

        public CreateShowTimeUseCase(IShowtimesRepository showtimesRepository, IMoviesClientApi moviesApi, IMoviesRepository moviesRepository) {
            _showtimesRepository = showtimesRepository;
            _moviesApi = moviesApi;
            _moviesRepository = moviesRepository;
        }

        public async Task<ShowtimeEntity> Execute(int auditoriumId, string movieId, DateTime sessionDate, CancellationToken cancel) {
            MovieEntity movie = await _moviesRepository.GetByExternalIdAsync(movieId, cancel).ConfigureAwait(false);

            if (movie != null) {
                return await SaveShowtime(auditoriumId, movie, sessionDate, cancel);
            }

            movie = await _moviesApi.GetById(movieId) ?? throw new EntityNotFoundException(movieId, nameof(MovieEntity));
            movie = await _moviesRepository.CreateAsync(movie).ConfigureAwait(false);
            return await SaveShowtime(auditoriumId, movie, sessionDate, cancel);
        }

        private async Task<ShowtimeEntity> SaveShowtime(int auditoriumId, MovieEntity movie, DateTime sessionDate, CancellationToken cancel) {
            var showtime = new ShowtimeEntity { AuditoriumId = auditoriumId, Movie = movie, SessionDate = sessionDate };
            return await _showtimesRepository.CreateShowtime(showtime, cancel);
        }
    }
}
