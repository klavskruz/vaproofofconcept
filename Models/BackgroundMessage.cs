using MeetingApi.Controllers.Service.JiraService;
using MeetingApi.Extras;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace MeetingApi.Models
{
    public class BackgroundMessage : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            // Find your Account Sid and Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            TaskTimer.schedule_Timer_MorningMessages();
        }
    }
}
