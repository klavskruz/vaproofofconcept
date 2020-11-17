using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models
{
    public class MeetingContext :DbContext
    {
        public MeetingContext(DbContextOptions<MeetingContext> options)
            : base(options)
        {

        }
        public DbSet<Meeting> Meetings { get; set; }
    }
}
