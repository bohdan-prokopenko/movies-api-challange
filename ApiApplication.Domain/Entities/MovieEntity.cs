using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Entities {
    [Serializable]
    public class MovieEntity {
        public int Id {
            get; set;
        }
        public string ExternalId {
            get; set;
        }
        public string Title {
            get; set;
        }
        public string ImdbId {
            get; set;
        }
        public string Stars {
            get; set;
        }
        public DateTime ReleaseDate {
            get; set;
        }
        public List<ShowtimeEntity> Showtimes {
            get; set;
        }

        public override bool Equals(object obj) {
            return obj is MovieEntity entity &&
                   Id == entity.Id &&
                   ExternalId == entity.ExternalId &&
                   Title == entity.Title &&
                   ImdbId == entity.ImdbId &&
                   Stars == entity.Stars &&
                   ReleaseDate == entity.ReleaseDate &&
                   EqualityComparer<List<ShowtimeEntity>>.Default.Equals(Showtimes, entity.Showtimes);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, ExternalId, Title, ImdbId, Stars, ReleaseDate, Showtimes);
        }
    }
}
