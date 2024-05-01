using ApiApplication.Domain.Entities;

using System;

namespace ApiApplication.Responses {
    public class CreateShowTimeResponseDto {
        public int Id {
            get; set;
        }
        public int MovieId {
            get; set;
        }
        public DateTime SessionDate {
            get; set;
        }
        public int AuditoriumId {
            get; set;
        }
        public CreateShowTimeResponseDto(ShowtimeEntity showtime) {
            Id = showtime.Id;
            MovieId = showtime.Movie.Id;
            AuditoriumId = showtime.AuditoriumId;
            SessionDate = showtime.SessionDate;
        }
    }
}
