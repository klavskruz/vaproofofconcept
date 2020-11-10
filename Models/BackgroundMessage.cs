using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MeetingApi.Models
{
    public class BackgroundMessage : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Find your Account Sid and Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string accountSid = "AC6ca0291c85bac2bf44ec8bd5df46e833";
                    string authToken = "60c9c687bd61e9917a929e7da6125e87";

                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Automatic message!",
                        from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                        to: new Twilio.Types.PhoneNumber("whatsapp:+447376483667")
                    );

                    Console.WriteLine(message.Sid);
                    Thread.Sleep(30000);
                }
                catch (Exception ex)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                }
            }
        }
    }
}
