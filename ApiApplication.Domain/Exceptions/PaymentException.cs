using System;

namespace ApiApplication.Domain.Exceptions {

    [Serializable]
    public class PaymentException : DomainException {
        public PaymentException() {
        }
        public PaymentException(string message) : base(message) { }
        public PaymentException(string message, Exception inner) : base(message, inner) { }

        public static PaymentException TicketAlreadyPaid() {
            var message = "The ticket has been already paid";
            return new PaymentException(message);
        }

        public static PaymentException ExpiredReservation() {
            var message = "The reservation has been expired";
            return new PaymentException(message);
        }
    }
}
