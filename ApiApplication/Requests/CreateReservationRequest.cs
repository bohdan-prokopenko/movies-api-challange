using System.Collections.Generic;

namespace ApiApplication.Requests {
    public class CreateReservationRequest {
        public List<short> Seats {
            get; set;
        }

        public short Row {
            get; set;
        }
    }
}
