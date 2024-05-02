using ApiApplication.Domain.Entities;
using ApiApplication.Domain.UseCases;
using ApiApplication.Requests;
using ApiApplication.Responses;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Controllers {
    [Route("api/auditorium/{auditoriumId}/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase {
        private readonly ICreateShowTimeUseCase _createShowTimeUseCase;

        public ShowTimesController(ICreateShowTimeUseCase createShowTimeUseCase) {
            _createShowTimeUseCase = createShowTimeUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<CreateShowTimeResponseDto>> CreateShowtime([FromRoute] int auditoriumId, [FromBody] CreateShowtimeRequest request, CancellationToken token) {
            ShowtimeEntity showtime = await _createShowTimeUseCase.Execute(auditoriumId, request.MovieId, request.SessionDate, token).ConfigureAwait(false);
            return new CreateShowTimeResponseDto(showtime);
        }
    }
}
