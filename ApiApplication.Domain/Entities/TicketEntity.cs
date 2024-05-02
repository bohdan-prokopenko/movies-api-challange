using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Entities {
    public class TicketEntity {
        public TicketEntity() {
            CreatedTime = DateTime.Now;
            Paid = false;
        }

        public Guid Id {
            get; set;
        }
        public int ShowtimeId {
            get; set;
        }
        public ICollection<SeatEntity> Seats {
            get; set;
        }
        public DateTime CreatedTime {
            get; set;
        }
        public bool Paid {
            get; set;
        }
        public ShowtimeEntity Showtime {
            get; set;
        }

        public override bool Equals(object obj) {
            return obj is TicketEntity entity &&
                   Id.Equals(entity.Id) &&
                   ShowtimeId == entity.ShowtimeId &&
                   EqualityComparer<ICollection<SeatEntity>>.Default.Equals(Seats, entity.Seats) &&
                   CreatedTime == entity.CreatedTime &&
                   Paid == entity.Paid &&
                   EqualityComparer<ShowtimeEntity>.Default.Equals(Showtime, entity.Showtime);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, ShowtimeId, Seats, CreatedTime, Paid, Showtime);
        }
    }
}
