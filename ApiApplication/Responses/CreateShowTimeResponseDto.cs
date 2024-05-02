using ApiApplication.Domain.Entities;
using ApiApplication.Dto;

using System;

namespace ApiApplication.Responses {
    public class CreateShowTimeResponseDto {
        public int Id {
            get; set;
        }
        public MovieDto Movie {
            get; set;
        }
        public DateTime SessionDate {
            get; set;
        }
        public int Auditorium {
            get; set;
        }
        public CreateShowTimeResponseDto(ShowtimeEntity showtime) {
            MovieEntity movie = showtime.Movie;
            Id = showtime.Id;
            Auditorium = showtime.AuditoriumId;
            SessionDate = showtime.SessionDate;
            Movie = new MovieDto {
                ImdbId = movie.ImdbId,
                ReleaseDate = movie.ReleaseDate,
                Stars = movie.Stars,
                Title = movie.Title,
            };
        }
    }
}
