using System;

namespace MeetingApi.Models
{
    public class Outboundleg
    {
        public int[] CarrierIds { get; set; }
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public DateTime DepartureDate { get; set; }
    }

   

  

}
