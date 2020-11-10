using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using MeetingApi.Models;


namespace MeetingApi.Models
{
    public class WeatherAPIContext
    {
       
        WeatherForecastModel forecast;
        string errorString;
        Location[] locations;
        IHttpClientFactory _factory;
        public WeatherAPIContext([FromServices] IHttpClientFactory factory)
        {
            _factory = factory;
        }
        
        public async Task<string> GetWoid(string cityName) { 
            var request = new HttpRequestMessage(HttpMethod.Get,
                    $"https://www.metaweather.com/api/location/search/?query={cityName}");
            var client = _factory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                locations = await response.Content.ReadFromJsonAsync<Location[]>();
                errorString = null;
                return locations[0].woeid.ToString();
            }
            else
            {
                errorString = $"There was an error while getting the woid. Reason : {response.ReasonPhrase}";
                return errorString;
            }
            
            
        }


        
        public async Task<WeatherForecastModel> GetWeatherFiveDays(string cityWoid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://www.metaweather.com/api/location/{cityWoid}/");
            var client = _factory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                forecast = await response.Content.ReadFromJsonAsync<WeatherForecastModel>();
                errorString = null;
                return forecast;
            }
            else
            {
                errorString = $"There was an error while getting the forecast. Reason : {response.ReasonPhrase}";
                throw new Exception();
            }
            
            
        }



    }
}
