
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using MeetingApi.Controllers.Helpers;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherDialogsController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherDialogsController : ControllerBase
    {

        WeatherAPIContext _context;
        public WeatherDialogsController([FromServices] IHttpClientFactory factory)
        {
            _context = new WeatherAPIContext(factory);
            
        }

        private static readonly JsonParser jsonParser = DialogService.returnNewJsonParser();
       

        [HttpPost]
        public async Task<ContentResult> GetWeatherAsync()
        {
            // Parse the body of the request using the Protobuf JSON parser,
            // *not* Json.NET.
            string textToReturn = "";
            string requestJson;
            using (TextReader reader = new StreamReader(Request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
            }

            WebhookRequest request;
            request = jsonParser.Parse<WebhookRequest>(requestJson);

            textToReturn = await WeatherService.getWeather(request, _context);

            string responseJson = DialogService.populateResponse(textToReturn);
            var content = Content(responseJson, "application/json");

            return content;
        }

    }
}

