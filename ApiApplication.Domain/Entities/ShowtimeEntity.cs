using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Entities {
    public class ShowtimeEntity {
        public int Id {
            get; set;
        }
        public MovieEntity Movie {
            get; set;
        }
        public DateTime SessionDate {
            get; set;
        }
        public int AuditoriumId {
            get; set;
        }
        public ICollection<TicketEntity> Tickets {
            get; set;
        }

        public override bool Equals(object obj) {
            return obj is ShowtimeEntity entity &&
                   Id == entity.Id &&
                   EqualityComparer<MovieEntity>.Default.Equals(Movie, entity.Movie) &&
                   SessionDate == entity.SessionDate &&
                   AuditoriumId == entity.AuditoriumId &&
                   EqualityComparer<ICollection<TicketEntity>>.Default.Equals(Tickets, entity.Tickets);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Movie, SessionDate, AuditoriumId, Tickets);
        }
    }
}
