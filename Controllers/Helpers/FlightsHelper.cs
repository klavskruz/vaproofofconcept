using Google.Cloud.Dialogflow.V2;
using MeetingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Controllers.Helpers
{
    public class FlightsHelper
    {
        static string textToReturn;
        public static async Task<string> getFlightPricesAsync(WebhookRequest request, FlightsAPIContext _apiContext) {
            // Extracts the parameters of the request
            var requestParameters = request.QueryResult.Parameters;
            string fromCity = requestParameters.Fields["from-city"].StringValue;
            string toCity = requestParameters.Fields["to-city"].StringValue;
            DateTime departure = DateTimeOffset.Parse(requestParameters.Fields["date-time"].StringValue).UtcDateTime.Date;
            // Calls SkyScanner API and gets the cheapest flight as a return 
            var flight = await _apiContext.GetFlight(fromCity, toCity, departure);
            
            if (flight != null)
            {
                textToReturn = $"The price is {flight.Quotes[0].MinPrice} euro " +
                $"Leaving from {flight.Places.Where(p => p.PlaceId == flight.Quotes[0].OutboundLeg.OriginId).First().Name} and " +
                $"going to {flight.Places.Where(p => p.PlaceId == flight.Quotes[0].OutboundLeg.DestinationId).First().Name}. " +
                $"Date: {flight.Quotes[0].OutboundLeg.DepartureDate.ToShortDateString()}. " +
                $"Direct: {flight.Quotes[0].Direct}." +
                " Would you like to buy this flight?";
            }

            else
            {
                textToReturn = "No flights found";
            }
            return textToReturn;
        }

        public static async Task<string> addFlightToDatabase(WebhookRequest request,FlightsAPIContext _apiContext, FlightContext _dbContext) {
            // Extracts the parameters of the request
            var requestParameters = request.QueryResult.Parameters;
            string fromCity = requestParameters.Fields["from-city"].StringValue;
            string toCity = requestParameters.Fields["to-city"].StringValue;
            
            DateTime departure = DateTimeOffset.Parse(requestParameters.Fields["date-time"].StringValue).UtcDateTime.Date;
            var flight = await _apiContext.GetFlight(fromCity, toCity, departure);

            if (flight.Quotes.Any())
            {
                FlightModel flightItem = new FlightModel
                {
                    FromCity = flight.Places.Where(p => p.PlaceId == flight.Quotes[0].OutboundLeg.OriginId).First().Name,
                    ToCity = flight.Places.Where(p => p.PlaceId == flight.Quotes[0].OutboundLeg.DestinationId).First().Name,
                    DepartureDate = flight.Quotes[0].OutboundLeg.DepartureDate.Date,
                    Price = flight.Quotes[0].MinPrice,


                };
                _dbContext.Add(flightItem);
                await _dbContext.SaveChangesAsync();
                textToReturn = "Done!";

            }
            else
            {
                textToReturn = "Could not add a flight";
            }
            return textToReturn;
        }

        public static async Task<string> showAllFlightsInDatabase(FlightContext _dbContext) {
            var allFlights = await _dbContext.Flights.ToListAsync();
           
            if (allFlights.Any())
            {
                textToReturn = "You have the following flights in your itenirary: " + System.Environment.NewLine;
                foreach (FlightModel flight in allFlights)
                {
                    textToReturn += $"From {flight.FromCity} to {flight.ToCity} on {flight.DepartureDate.ToShortDateString()}." + System.Environment.NewLine;
                }
            }
            else
            {
                textToReturn = "There are no flights to display!";
            }
            return textToReturn;
        }

        public static async Task<string> deleteFlightFromDatabase(WebhookRequest request,FlightContext _dbContext) {
            var requestParameters = request.QueryResult.Parameters;
            
            string fromCity = requestParameters.Fields["from-city"].StringValue;
            string toCity = requestParameters.Fields["to-city"].StringValue;
            DateTime departure = DateTimeOffset.Parse(requestParameters.Fields["date-time"].StringValue).UtcDateTime.Date;
            var flight = await _dbContext.Flights.FirstOrDefaultAsync(f => f.FromCity.ToLower().Contains(fromCity.ToLower())
            && f.ToCity.ToLower().Contains(toCity.ToLower())
            && f.DepartureDate.Date == departure
            );


            if (flight == null)
            {
                textToReturn = "There is no such flight";
            }
            else
            {
                _dbContext.Flights.Remove(flight);
                await _dbContext.SaveChangesAsync();
                textToReturn = "Cancelled!";
            }
            return textToReturn;
        }
        
    }
}
