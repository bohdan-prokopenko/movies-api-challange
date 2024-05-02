using System;

namespace ApiApplication.Requests {
    public class CreateShowtimeRequest {
        public int Auditorium {
            get; set;
        }
        public string MovieId {
            get; set;
        }
        public DateTime SessionDate {
            get; set;
        }
    }
}
