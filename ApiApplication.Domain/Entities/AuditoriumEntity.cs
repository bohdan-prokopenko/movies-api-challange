using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Entities {
    public class AuditoriumEntity {
        public int Id {
            get; set;
        }
        public List<ShowtimeEntity> Showtimes {
            get; set;
        }
        public ICollection<SeatEntity> Seats {
            get; set;
        }

        public override bool Equals(object obj) {
            return obj is AuditoriumEntity entity &&
                   Id == entity.Id &&
                   EqualityComparer<List<ShowtimeEntity>>.Default.Equals(Showtimes, entity.Showtimes) &&
                   EqualityComparer<ICollection<SeatEntity>>.Default.Equals(Seats, entity.Seats);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Showtimes, Seats);
        }
    }
}
