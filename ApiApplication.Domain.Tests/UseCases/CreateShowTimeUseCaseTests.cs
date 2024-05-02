using ApiApplication.Domain.Api;
using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Repositories;
using ApiApplication.Domain.UseCases;

using Moq;

using NUnit.Framework;

using System;
using System.Threading;
using System.Threading.Tasks;

using static System.Threading.CancellationToken;

namespace ApiApplication.Domain.Tests.UseCases {

    [TestFixture]
    internal class CreateShowTimeUseCaseTests {
        private Mock<IShowtimesRepository> _repostioryMock;
        private Mock<IMoviesClientApi> _moviesClientApiMock;
        private Mock<IMoviesRepository> _moviesRepositoryMock;
        private ICreateShowTimeUseCase _createShowTimeUseCase;

        [SetUp]
        public void Setup() {
            _repostioryMock = new Mock<IShowtimesRepository>();
            _moviesClientApiMock = new Mock<IMoviesClientApi>();
            _moviesRepositoryMock = new Mock<IMoviesRepository>();
            _createShowTimeUseCase = new CreateShowTimeUseCase(_repostioryMock.Object, _moviesClientApiMock.Object, _moviesRepositoryMock.Object);
        }

        [Test(Description = "When movie exists should create showtime with the existing movie")]
        public async Task ExecuteTest1() {
            DateTime sessionDate = DateTime.Now;
            var auditoriumId = 1;
            var existingMovie = new MovieEntity { Title = "Test" };
            var showtime = new ShowtimeEntity { AuditoriumId = auditoriumId, Movie = existingMovie, SessionDate = sessionDate };
            var expectedResult = new ShowtimeEntity { AuditoriumId = auditoriumId, Movie = existingMovie, SessionDate = sessionDate, Id = 1 };

            _ = _moviesRepositoryMock.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingMovie);
            _ = _repostioryMock.Setup(r => r.CreateShowtime(showtime, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            ShowtimeEntity result = await _createShowTimeUseCase.Execute(auditoriumId, "someExternalMovieId", sessionDate, None);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test(Description = "When movie does not exist should call movies api and save movie entity")]
        public async Task ExecuteTest2() {
            DateTime sessionDate = DateTime.Now;
            var auditoriumId = 1;
            var movie = new MovieEntity { Title = "Test" };
            var expectedMovie = new MovieEntity { Title = "Test", Id = 1 };
            var showtime = new ShowtimeEntity { AuditoriumId = auditoriumId, Movie = expectedMovie, SessionDate = sessionDate };
            var expectedResult = new ShowtimeEntity { AuditoriumId = auditoriumId, Movie = expectedMovie, SessionDate = sessionDate, Id = 1 };

            _ = _moviesClientApiMock.Setup(r => r.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(movie);
            _ = _moviesRepositoryMock.Setup(r => r.CreateAsync(movie, It.IsAny<CancellationToken>())).ReturnsAsync(expectedMovie);
            _ = _repostioryMock.Setup(r => r.CreateShowtime(showtime, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            ShowtimeEntity result = await _createShowTimeUseCase.Execute(auditoriumId, "someExternalMovieId", sessionDate, None);

            Assert.That(result, Is.EqualTo(expectedResult));
            _moviesClientApiMock.Verify(a => a.GetById("someExternalMovieId", It.IsAny<CancellationToken>()), Times.Once);
            _moviesRepositoryMock.Verify(a => a.CreateAsync(movie, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test(Description = "When api returns null should throw EntityNotFoundException")]
        public void ExecuteTest3() {
            DateTime sessionDate = DateTime.Now;
            var auditoriumId = 1;
            var movieId = "someExternalMovieId";

            _ = _moviesRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<MovieEntity>(), It.IsAny<CancellationToken>())).ThrowsAsync(new EntityNotFoundException("someExternalMovieId", nameof(MovieEntity)));

            _ = Assert.ThrowsAsync<EntityNotFoundException>(() => _createShowTimeUseCase.Execute(auditoriumId, movieId, sessionDate, None));

            _moviesRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<MovieEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test(Description = "When an unexpected exception occurs during the process shoul throw DomainException")]
        public void ExecuteTest4() {
            DateTime sessionDate = DateTime.Now;
            var auditoriumId = 1;
            var movieId = "someExternalMovieId";
            var existingMovie = new MovieEntity { Title = "Test" };

            _ = _moviesRepositoryMock.Setup(r => r.GetByExternalIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingMovie);
            _ = _repostioryMock.Setup(r => r.CreateShowtime(It.IsAny<ShowtimeEntity>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("someExternalMovieId"));

            _ = Assert.ThrowsAsync<DomainException>(() => _createShowTimeUseCase.Execute(auditoriumId, movieId, sessionDate, None));
        }
    }
}
