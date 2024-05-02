using ApiApplication.Domain.Entities;
using ApiApplication.Domain.UseCases;
using ApiApplication.Requests;
using ApiApplication.Responses;

using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase {
        private readonly ICreateReservationUseCase createReservationUseCase;

        public ReservationsController(ICreateReservationUseCase createReservationUseCase) {
            this.createReservationUseCase = createReservationUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<CreateReservationResponse>> CreateReservation([FromRoute] int showtimeId, [FromBody] CreateReservationRequest request, CancellationToken token) {
            TicketEntity ticket = await createReservationUseCase.Execute(showtimeId, request.Row, request.Seats, token).ConfigureAwait(false);
            return new CreateReservationResponse(ticket);
        }
    }
}
