using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models
{
    public class FlightContext :DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options)
            : base(options)
        {
            
        }
        public DbSet<FlightModel> Flights { get; set; }
    }
}
