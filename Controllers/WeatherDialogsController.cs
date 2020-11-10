
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Google.Type;
using MeetingApi.Controllers;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
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
        // A Protobuf JSON parser configured to ignore unknown fields. This makes
        // the action robust against new fields being introduced by Dialogflow.
        private static readonly JsonParser jsonParser =
        new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

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
            //Parse the intent params
            request = jsonParser.Parse<WebhookRequest>(requestJson);
            var requestParameters = request.QueryResult.Parameters;
            string location = requestParameters.Fields["geo-city"].StringValue;
            System.DateTime datetimeFromParams = DateTimeOffset.Parse(requestParameters.Fields["date-time"].StringValue).UtcDateTime;
            System.DateTime date = datetimeFromParams.Date;
            string locationCapitalised = char.ToUpper(location[0]) + location.Substring(1);
            string woid = await _context.GetWoid(location);
            var forecast = await _context.GetWeatherFiveDays(woid);

            // GET WEATHER TODAY
            if (date == System.DateTime.Today)
            {           
                textToReturn = $"Today's forecast in {locationCapitalised} is {forecast.Consolidated_weather[0].Weather_state_name.ToLower()}." +
                    $" Min. temperature is {forecast.Consolidated_weather[0].Min_temp}c and the" +
                    $" max temperature is {forecast.Consolidated_weather[0].Max_temp}c.";

            }
            //GET WEATHER TOMORROW
            else if ((System.DateTime.Today - date).TotalDays == -1)
            { 
                textToReturn = $"Tomorrow  in {locationCapitalised} you can expect {forecast.Consolidated_weather[1].Weather_state_name.ToLower()}." +
                    $" Min. temperature is {forecast.Consolidated_weather[1].Min_temp}c and the" +
                    $" max temperature is {forecast.Consolidated_weather[1].Max_temp}c.";

            }
            else
            {
                textToReturn = "Something has gone terribly wrong mate!";
            }

            // Populate the response
            WebhookResponse response = new WebhookResponse
            {
                FulfillmentText = textToReturn
            };
            // Ask Protobuf to format the JSON to return.
         
            string responseJson = response.ToString();
            return Content(responseJson, "application/json");
        }

    }
}

