using ApiApplication.Domain.Entities;
using ApiApplication.Dto;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiApplication.Responses {
    public class CreateReservationResponse {
        public Guid Id {
            get; private set;
        }
        public int Auditorium {
            get; set;
        }

        public List<short> Seats {
            get; set;
        }

        public DateTime ShowTime {
            get; private set;
        }

        public MovieDto Movie {
            get; set;
        }

        public CreateReservationResponse(TicketEntity ticket) {
            MovieEntity movie = ticket.Showtime.Movie;
            Id = ticket.Id;
            ShowTime = ticket.Showtime.SessionDate;
            Auditorium = ticket.Showtime.AuditoriumId;
            Seats = ticket.Seats.Select(s => s.SeatNumber).ToList();
            Movie = new MovieDto {
                ImdbId = movie.ImdbId,
                ReleaseDate = movie.ReleaseDate,
                Stars = movie.Stars,
                Title = movie.Title,
            };
        }
    }
}
