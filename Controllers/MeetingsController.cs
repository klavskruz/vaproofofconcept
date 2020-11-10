using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingApi.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace MeetingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly MeetingContext _context;

        public MeetingsController(MeetingContext context)
        {
            _context = context;
        }

        // GET: api/Meetings
        [HttpGet]
        public void SendMessage()
        {
            // Find your Account Sid and Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            string accountSid = "AC6ca0291c85bac2bf44ec8bd5df46e833";
            string authToken = "60c9c687bd61e9917a929e7da6125e87";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Hello there!",
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+447376483667")
            );

            Console.WriteLine(message.Sid);
        }

        // GET: api/Meetings
        [HttpPost]
        public async Task<HttpResponseMessage> JiraFunctionAsync()
        {
         

            
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://vapoc.atlassian.net/rest/api/3/search?jql=project=VAPOC"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");

                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("klavs.kruzins@gmail.com:fpd7JDMRYwa4aRmRPH8uD0CB"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    var response = await httpClient.SendAsync(request);
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return response;
                }

            }



        }
    }
}



