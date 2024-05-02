using System;
using System.Collections.Generic;

namespace ApiApplication.Domain.Entities {
    public class SeatEntity {
        public short Row {
            get; set;
        }
        public short SeatNumber {
            get; set;
        }
        public int AuditoriumId {
            get; set;
        }
        public AuditoriumEntity Auditorium {
            get; set;
        }

        public override bool Equals(object obj) {
            return obj is SeatEntity entity &&
                   Row == entity.Row &&
                   SeatNumber == entity.SeatNumber &&
                   AuditoriumId == entity.AuditoriumId &&
                   EqualityComparer<AuditoriumEntity>.Default.Equals(Auditorium, entity.Auditorium);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Row, SeatNumber, AuditoriumId, Auditorium);
        }
    }
}
