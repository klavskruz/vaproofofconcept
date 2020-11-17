using Google.Cloud.Dialogflow.V2;
using MeetingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Controllers.Helpers
{
    public class WeatherService
    {
        public static async Task<string> getWeather(WebhookRequest request, WeatherAPIContext _context)
        {
            string textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
            string location = requestParameters.Fields["geo-city"].StringValue;
            DateTime date= DateTimeOffset.Parse(requestParameters.Fields["date-time"].StringValue).UtcDateTime.Date;
            string locationCapitalised = char.ToUpper(location[0]) + location.Substring(1);
            string woid = await _context.GetWoid(location);
            var forecast = await _context.GetWeatherFiveDays(woid);
            bool isToday = date == DateTime.Today;
            bool isTomorrow = (DateTime.Today - date).TotalDays == -1;

            // Get weather for today
            if (isToday)
            {
                textToReturn = $"Today's forecast in {locationCapitalised} is {forecast.Consolidated_weather[0].Weather_state_name.ToLower()}." +
                    $" Min. temperature is {forecast.Consolidated_weather[0].Min_temp}c and the" +
                    $" max temperature is {forecast.Consolidated_weather[0].Max_temp}c.";

            }
            // Get weather for tomorrow
            else if (isTomorrow)
            {
                textToReturn = $"Tomorrow  in {locationCapitalised} you can expect {forecast.Consolidated_weather[1].Weather_state_name.ToLower()}." +
                    $" Min. temperature is {forecast.Consolidated_weather[1].Min_temp}c and the" +
                    $" max temperature is {forecast.Consolidated_weather[1].Max_temp}c.";

            }
            else
            {
                textToReturn = "Given date could not be resolved";
            }
            return textToReturn;
        }
    }
}
