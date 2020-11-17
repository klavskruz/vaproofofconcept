
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MeetingApi.Controllers.Helpers;

namespace FlightDialogsController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightDialogsController : ControllerBase
    {
        FlightContext _dbContext;
        FlightsAPIContext _apiContext;


        public FlightDialogsController([FromServices] IHttpClientFactory factory, FlightContext context)
        {
            _apiContext = new FlightsAPIContext(factory);
            _dbContext = context;

        }

        private static readonly JsonParser jsonParser = DialogService.returnNewJsonParser();


        [HttpPost]
        public async Task<ContentResult> FlightsAsync()
        {

            string textToReturn;
            WebhookRequest request;

            // Parse the body of the request using the Protobuf JSON parser,
            // not Json.NET.  
            string requestJson;
            using (TextReader reader = new StreamReader(Request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
            }
            

            //Parse the intent params
            request = jsonParser.Parse<WebhookRequest>(requestJson);

            // Get flight quote
            if (request.QueryResult.Action.Equals("findFlight"))
            {
                textToReturn = await FlightsService.getFlightPricesAsync(request, _apiContext);
            }

            // Add flight to database
            else if (request.QueryResult.Action.Equals("buyFlight")) {
                textToReturn = await FlightsService.addFlightToDatabase(request, _apiContext, _dbContext);
            }

            // See all flights in database
            else if (request.QueryResult.Action == "showAll")
            {
                textToReturn = await FlightsService.showAllFlightsInDatabase(_dbContext);
            }

            // Remove flight from the database
            else if (request.QueryResult.Action == "deleteFlight")
            {
                textToReturn = await FlightsService.deleteFlightFromDatabase(request, _dbContext);

            }


            else {
                textToReturn = "Something has gone wrong";
            }


            string responseJson = DialogService.populateResponse(textToReturn);
            var content = Content(responseJson, "application/json");

            return content;
            }

        }
    }

