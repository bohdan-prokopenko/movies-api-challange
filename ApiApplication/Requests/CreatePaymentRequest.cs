using System;

namespace ApiApplication.Requests {
    public class CreatePaymentRequest {
        public Guid Reservation {
            get; set;
        }
    }
}
