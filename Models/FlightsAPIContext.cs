using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;



namespace MeetingApi.Models
{
    public class FlightsAPIContext
    {
        IHttpClientFactory _factory;
        Flight flight;

        public FlightsAPIContext([FromServices]IHttpClientFactory factory)
        {
            _factory = factory;
        }

     
        public async Task<string> GetAirportId(string city) {
            string uriString = $"https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/autosuggest/v1.0/LV/EUR/en-US/?query={city}";
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(uriString),
                Method = HttpMethod.Get,
                Headers = {
                        { "X-Version", "1" },
                        {"x-rapidapi-host", "skyscanner-skyscanner-flight-search-v1.p.rapidapi.com" },
                        { "x-rapidapi-key", "bf7aa826admsh914e2032ba16661p12d507jsnd32e0f8f48a5"

                       }}
            };

            var client = _factory.CreateClient();
            var response = await client.SendAsync(request);
            var places = await response.Content.ReadFromJsonAsync<PlacesList>();
            if (places.Places.Any())
            {
                return places.Places[0].PlaceId;
            }
            else {
                return "No codes found";  
            }
            
                      
        }

        
        
        public async Task<Flight> GetFlight(string fromCity, string toCity,DateTime leavingDate)
        {
            string fromAirport = await this.GetAirportId(fromCity);
            string toAirport = await this.GetAirportId(toCity);
            string outboundpartialdate = leavingDate.Date.ToString("yyyy-MM-dd");
            string country = "US";
            string currency = "EUR";
            string locale = "en-US";            
            string uriString = $"https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/browsequotes/v1.0/{country}/{currency}/{locale}/{fromAirport}/{toAirport}/{outboundpartialdate}";   
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(uriString),
                Method = HttpMethod.Get,
                Headers = {
                        { "X-Version", "1" },
                        {"x-rapidapi-host", "skyscanner-skyscanner-flight-search-v1.p.rapidapi.com" },
                        { "x-rapidapi-key", "bf7aa826admsh914e2032ba16661p12d507jsnd32e0f8f48a5"}
                       }};
            var client = _factory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                flight = await response.Content.ReadFromJsonAsync<Flight>();
                return flight;
            }
            else {
                return null;
            }
            
        }

        
    }
}
