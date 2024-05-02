using ApiApplication.Domain.Entities;

using System;

namespace ApiApplication.Responses {
    public class CreatePaymentResponse {
        public Guid Payment {
            get; set;
        }

        public bool Paid {
            get; set;
        }

        public CreatePaymentResponse(TicketEntity ticket) {
            Payment = ticket.Id;
            Paid = ticket.Paid;
        }
    }
}
