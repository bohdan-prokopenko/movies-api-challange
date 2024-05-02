using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiApplication.Domain.Exceptions {

    [Serializable]
    public class ReservationException : DomainException {
        public ReservationException() {
        }
        public ReservationException(string message) : base(message) { }
        public ReservationException(string message, Exception inner) : base(message, inner) { }

        public static ReservationException SeatsAreNotContiguous(IEnumerable<short> seatNumbers) {
            var message = "Requested seats are not contiguous: {0}";
            return new ReservationException(string.Format(message, string.Join(", ", seatNumbers)));
        }

        public static ReservationException SeatsAreAlredyReserved(IEnumerable<short> seatNumbers) {
            var message = "Requested seats are already reserved: {0}";
            return new ReservationException(string.Format(message, string.Join(", ", seatNumbers)));
        }
    }
}
