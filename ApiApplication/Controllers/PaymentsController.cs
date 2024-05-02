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

    public class PaymentsController : ControllerBase {
        private readonly ICreatePaymentUseCase _createPaymentUseCase;

        public PaymentsController(ICreatePaymentUseCase createPaymentUseCase) {
            _createPaymentUseCase = createPaymentUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<CreatePaymentResponse>> CreateReservation([FromBody] CreatePaymentRequest request, CancellationToken token) {
            TicketEntity ticket = await _createPaymentUseCase.Execute(request.Reservation, token).ConfigureAwait(false);
            return new CreatePaymentResponse(ticket);
        }
    }
}
