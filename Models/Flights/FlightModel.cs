using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models
{
    public class FlightModel
    {
        public int Id { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public decimal Price { get; set; }
        public DateTime DepartureDate { get; set; }

    }
}
