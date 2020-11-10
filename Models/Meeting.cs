using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Person { get; set; }
        public bool Mandatory { get; set; }
        public string Location { get; set; }

    }
}
