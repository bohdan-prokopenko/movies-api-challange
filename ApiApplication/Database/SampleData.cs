using ApiApplication.Domain.Entities;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;

namespace ApiApplication.Database {
    public class SampleData {
        public static void Initialize(IApplicationBuilder app) {
            using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            CinemaContext context = serviceScope.ServiceProvider.GetService<CinemaContext>();
            _ = context.Database.EnsureCreated();

            _ = context.Auditoriums.Add(new AuditoriumEntity {
                Id = 1,
                Showtimes = new List<ShowtimeEntity>
                {
                    new ShowtimeEntity
                    {
                        Id = 1,
                        SessionDate = new DateTime(2023, 1, 1),
                        Movie = new MovieEntity
                        {
                            Id = 1,
                            Title = "Inception",
                            ImdbId = "tt1375666",
                            ReleaseDate = new DateTime(2010, 01, 14),
                            Stars = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe"
                        },
                        AuditoriumId = 1,
                    }
                },
                Seats = GenerateSeats(1, 28, 22)
            });

            _ = context.Auditoriums.Add(new AuditoriumEntity {
                Id = 2,
                Seats = GenerateSeats(2, 21, 18)
            });

            _ = context.Auditoriums.Add(new AuditoriumEntity {
                Id = 3,
                Seats = GenerateSeats(3, 15, 21)
            });

            _ = context.SaveChanges();
        }

        private static List<SeatEntity> GenerateSeats(int auditoriumId, short rows, short seatsPerRow) {
            var seats = new List<SeatEntity>();
            for (short r = 1; r <= rows; r++) {
                for (short s = 1; s <= seatsPerRow; s++) {
                    seats.Add(new SeatEntity { AuditoriumId = auditoriumId, Row = r, SeatNumber = s });
                }
            }

            return seats;
        }
    }
}
