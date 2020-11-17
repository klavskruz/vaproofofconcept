using MeetingApi.Controllers.Service.JiraService;
using MeetingApi.Controllers.Service.TwilioService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace MeetingApi.Extras
{
    public class TaskTimer
    {
        JiraAPIContext _context;
        static Timer timer;

       


        public static void schedule_Timer_MorningMessages()
        {
            Console.WriteLine("### Timer Started ###");

            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 13, 40, 0, 0); //Specify your scheduled time HH,MM,SS [8am and 42 minutes]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            timer = new Timer(tickTime);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        static async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("### Timer Stopped ### \n");
            timer.Stop();
            Console.WriteLine("### Scheduled Task Started ### \n\n");
            await TwilioService.SendMorningMessagesAsync();
            Console.WriteLine("### Task Finished ### \n\n");
            schedule_Timer_MorningMessages();
        }
    }
}

