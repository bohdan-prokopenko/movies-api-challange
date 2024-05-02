using ApiApplication.Domain.Entities;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Repositories;
using ApiApplication.Domain.UseCases;

using Moq;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Domain.Tests.UseCases {
    [TestFixture]
    internal class CreatePaymentUseCaseTests {
        private Mock<ITicketsRepository> _ticketsRepositoryMock;
        private ICreatePaymentUseCase _createPaymentUseCase;

        [SetUp]
        public void Setup() {
            _ticketsRepositoryMock = new Mock<ITicketsRepository>();
            _createPaymentUseCase = new CreatePaymentUseCase(_ticketsRepositoryMock.Object);
        }

        [Test(Description = "When reservation not found should throw EntityNotFoundException")]
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
            var ticket = new TicketEntity {
                Seats = seats,
            };

            var id = Guid.NewGuid();

            Assert.Multiple(() => {
                EntityNotFoundException ex = Assert.ThrowsAsync<EntityNotFoundException>(() => _createPaymentUseCase.Execute(id));
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo($"Entity TicketEntity not found. Entity ID: {id}"));
            });
        }

        [Test(Description = "When reservation has been already paid should throw PaymentException")]
        public void ExecuteTest3() {
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
            var ticket = new TicketEntity {
                Seats = seats,
                Paid = true,
            };

            var id = Guid.NewGuid();

            _ = _ticketsRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(ticket);

            Assert.Multiple(() => {
                PaymentException ex = Assert.ThrowsAsync<PaymentException>(() => _createPaymentUseCase.Execute(id));
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo($"The ticket has been already paid"));
            });
        }

        [Test(Description = "When reservation has been expired should throw PaymentException")]
        public void ExecuteTest4() {
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
            var ticket = new TicketEntity {
                Seats = seats,
                CreatedTime = DateTime.UtcNow.AddMinutes(-10),
            };

            var id = Guid.NewGuid();

            _ = _ticketsRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(ticket);

            Assert.Multiple(() => {
                PaymentException ex = Assert.ThrowsAsync<PaymentException>(() => _createPaymentUseCase.Execute(id));
                Assert.That(ex, Is.Not.Null);
                Assert.That(ex.Message, Is.EqualTo($"The reservation has been expired"));
            });
        }

        [Test(Description = "Reservation should be confirmed successfully")]
        public async Task ExecuteTest5() {
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
            var ticket = new TicketEntity {
                Seats = seats,
            };

            var id = Guid.NewGuid();

            _ = _ticketsRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(ticket);
            _ = _ticketsRepositoryMock.Setup(x => x.ConfirmPaymentAsync(It.IsAny<TicketEntity>(), It.IsAny<CancellationToken>())).ReturnsAsync(new TicketEntity { Paid = true });

            TicketEntity result = await _createPaymentUseCase.Execute(id);

            Assert.Multiple(() => {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Paid, Is.True);
            });

            _ticketsRepositoryMock.Verify(s => s.ConfirmPaymentAsync(It.IsAny<TicketEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
