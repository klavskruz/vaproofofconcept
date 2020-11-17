using System;

namespace MeetingApi.Models
{
    public class Quote
    {
        public int QuoteId { get; set; }
        public int MinPrice { get; set; }
        public bool Direct { get; set; }
        public Outboundleg OutboundLeg { get; set; }
        public DateTime QuoteDateTime { get; set; }
    }

   

  

}
