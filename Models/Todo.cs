using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Urgent { get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; }

    }
}
