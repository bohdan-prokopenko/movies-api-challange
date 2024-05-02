using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Repositories;
using ApiApplication.Domain.UseCases;

using Moq;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.Tests.UseCases {

    [TestFixture]
    internal class CreateReservationUseCaseTests {
        private Mock<IAuditoriumsRepository> _auditoriumsRepositoryMock;
        private Mock<IShowtimesRepository> _showtimesRepositoryMock;
        private Mock<ITicketsRepository> _ticketsRepositoryMock;
        private CreateReservationUseCase _createReservationUseCase;

        [SetUp]
        public void Setup() {
            _auditoriumsRepositoryMock = new Mock<IAuditoriumsRepository>();
            _ticketsRepositoryMock = new Mock<ITicketsRepository>();
            _showtimesRepositoryMock = new Mock<IShowtimesRepository>();
            _createReservationUseCase = new CreateReservationUseCase(_auditoriumsRepositoryMock.Object, _ticketsRepositoryMock.Object, _showtimesRepositoryMock.Object);
        }

        [Test(Description = "When seats are not exist in auditorium should throw EntityNotFoundException")]
        public void ExecuteTest2() {
            var seats = new List<SeatEntity>() {
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 3,
                },
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 2,
                },
            };
            var showtime = new ShowtimeEntity { Id = 1 };
            var auditorium = new AuditoriumEntity {
                Seats = seats,

            };

            _ = _showtimesRepositoryMock.Setup(r => r.GetWithTicketsByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(showtime);
            _ = _auditoriumsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(auditorium);

            Assert.Multiple(() => {
                EntityNotFoundException ex = Assert.ThrowsAsync<EntityNotFoundException>(() => _createReservationUseCase.Execute(1, 1, new List<short> { 4, 5 }));
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Entity SeatEntity not found. Entity ID: 1"));
            });
        }

        [Test(Description = "When seats are not contiguous should throw ReservationException")]
        public void ExecuteTest3() {
            var seats = new List<SeatEntity>() {
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 3,
                },
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 5,
                },
                 new SeatEntity {
                    Row = 1,
                    SeatNumber = 6,
                },
            };
            var showtime = new ShowtimeEntity { Id = 1 };
            var auditorium = new AuditoriumEntity {
                Seats = seats
            };

            _ = _showtimesRepositoryMock.Setup(r => r.GetWithTicketsByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(showtime);
            _ = _auditoriumsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(auditorium);

            Assert.Multiple(() => {
                ReservationException ex = Assert.ThrowsAsync<ReservationException>(() => _createReservationUseCase.Execute(1, 1, seats.Select(s => s.SeatNumber)));
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Requested seats are not contiguous: 3, 5, 6"));
            });
        }

        [Test(Description = "When showtime doesn't exist should throw EntityNotFoundException")]
        public void ExecuteTest4() {
            var seats = new List<SeatEntity>() {
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 4,
                },
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 5,
                },
                 new SeatEntity {
                    Row = 1,
                    SeatNumber = 6,
                },
            };
            var tickets = new List<TicketEntity> {
                new TicketEntity {
                    Seats=seats,
                }
            };
            var auditorium = new AuditoriumEntity {
                Seats = seats
            };

            _ = _auditoriumsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(auditorium);

            Assert.Multiple(() => {
                EntityNotFoundException ex = Assert.ThrowsAsync<EntityNotFoundException>(() => _createReservationUseCase.Execute(1, 1, seats.Select(s => s.SeatNumber)));
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Entity ShowtimeEntity not found. Entity ID: 1"));
            });
        }

        [Test(Description = "When seats contains already reserved seat should throw ReservationException")]
        public void ExecuteTest5() {
            var seats = new List<SeatEntity>() {
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 4,
                },
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 5,
                },
                 new SeatEntity {
                    Row = 1,
                    SeatNumber = 6,
                },
            };
            var tickets = new List<TicketEntity> {
                new TicketEntity {
                    Seats = seats,
                    Paid = true,
                }
            };
            var showTime = new ShowtimeEntity {
                AuditoriumId = 1,
                Tickets = tickets,
            };
            var auditorium = new AuditoriumEntity {
                Seats = seats,
                Showtimes = new List<ShowtimeEntity> { showTime }
            };

            _ = _ticketsRepositoryMock.Setup(r => r.GetEnrichedAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(tickets);
            _ = _showtimesRepositoryMock.Setup(r => r.GetWithTicketsByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(showTime);
            _ = _auditoriumsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(auditorium);


            Assert.Multiple(() => {
                ReservationException ex = Assert.ThrowsAsync<ReservationException>(() => _createReservationUseCase.Execute(1, 1, seats.Select(s => s.SeatNumber)));
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo("Requested seats are already reserved: 4, 5, 6"));
            });
        }

        [Test(Description = "Should create reservation")]
        public async Task ExecuteTest6() {
            var seats = new List<SeatEntity>() {
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 4,
                },
                new SeatEntity {
                    Row = 1,
                    SeatNumber = 5,
                },
                 new SeatEntity {
                    Row = 1,
                    SeatNumber = 6,
                },
            };
            var showTime = new ShowtimeEntity {
                AuditoriumId = 1,
            };
            var auditorium = new AuditoriumEntity {
                Seats = seats,
                Showtimes = new List<ShowtimeEntity> { showTime }
            };
            var ticket = new TicketEntity {
                Seats = seats,
            };

            _ = _showtimesRepositoryMock.Setup(r => r.GetWithTicketsByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(showTime);
            _ = _auditoriumsRepositoryMock.Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(auditorium);
            _ = _ticketsRepositoryMock.Setup(r => r.CreateAsync(showTime, seats, It.IsAny<CancellationToken>())).ReturnsAsync(ticket);

            TicketEntity result = await _createReservationUseCase.Execute(1, 1, seats.Select(s => s.SeatNumber));

            Assert.Multiple(() => {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.Not.EqualTo(default));
                Assert.That(result, Is.EqualTo(ticket));
            });

            _ticketsRepositoryMock.Verify(r => r.CreateAsync(showTime, seats, It.IsAny<CancellationToken>()));
        }
    }
}
