using ApiApplication.Domain.Entities;

using System;

namespace ApiApplication.Client.Http {
    internal class MovieDto {
        public string Id {
            get; set;
        }

        public string Rank {
            get; set;
        }
        public string Title {
            get; set;
        }

        public string Year {
            get; set;
        }

        public string Image {
            get; set;
        }

        public string Crew {
            get; set;
        }

        public string ImDbRating {
            get; set;
        }

        public string ImDbRatingCount {
            get; set;
        }

        public MovieEntity ToEntity() {
            var success = int.TryParse(Year, out var year);
            var entity = new MovieEntity {
                ExternalId = Id,
                Title = Title,
                ImdbId = ImDbRating,
                Stars = ImDbRatingCount,
            };

            if (success) {
                entity.ReleaseDate = new DateTime(year, 1, 1);
            }

            return entity;
        }
    }
}
