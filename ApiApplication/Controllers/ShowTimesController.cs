using ApiApplication.Domain.Entities;
using ApiApplication.Domain.UseCases;
using ApiApplication.Responses;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Controllers {
    [Route("api/auditorium/{auditoriumId}/movies/{movieId}/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase {
        private readonly ICreateShowTimeUseCase _createShowTimeUseCase;

        public ShowTimesController(ICreateShowTimeUseCase createShowTimeUseCase) {
            _createShowTimeUseCase = createShowTimeUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<CreateShowTimeResponseDto>> CreateShowtime([FromRoute] int auditoriumId, [FromRoute] string movieId, [FromBody] DateTime sessionDate, CancellationToken token) {
            ShowtimeEntity showtime = await _createShowTimeUseCase.Execute(auditoriumId, movieId, sessionDate, token).ConfigureAwait(false);
            return new CreateShowTimeResponseDto(showtime);
        }
    }
}
