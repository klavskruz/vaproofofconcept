using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using MeetingApi.Controllers.Helpers;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingDialogsController : ControllerBase
    {
        private readonly MeetingContext _context;

        public MeetingDialogsController(MeetingContext context)
        {
            _context = context;
        }

        private static readonly JsonParser jsonParser = DialogHelper.returnNewJsonParser();
        


        [HttpPost]
        public async Task<ContentResult> DialogActionAsync()
        {
            // Parse the body of the request using the Protobuf JSON parser,
            // *not* Json.NET.
            string textToReturn;
            string requestJson;
            using (TextReader reader = new StreamReader(Request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
            }

            WebhookRequest request;
            
            request = jsonParser.Parse<WebhookRequest>(requestJson);
            var requestParameters = request.QueryResult.Parameters;


            // Add a meeting into database
            if (request.QueryResult.Action == "addMeeting")
            {
                textToReturn = await MeetingHelper.AddMeetingToDatabase(request, _context);
            }
            // DISPLAY ALL MEETINGS 
            else if (request.QueryResult.Action == "showAll")
            {
                textToReturn = await MeetingHelper.ShowAllMeetings(request, _context);
            }

            // DELETE MEETING PROVIDED THE TIME
            else if (request.QueryResult.Action == "deleteMeetingTime")
            {
                textToReturn = await MeetingHelper.DeleteMeetingFromDatabase(requestParameters, _context);             
            }
            // FIND ALL MEETINGS WITH A CERTAIN PERSON
            else if (request.QueryResult.Action == "findByPerson")
            {
                textToReturn = await MeetingHelper.FindMeetingsFromDatabase(requestParameters, _context);
            }


            else {
                textToReturn = "Something has gone terribly wrong mate!";

            }

            string responseJson = DialogHelper.populateResponse(textToReturn);
            var content = Content(responseJson, "application/json");

            return content;
        }

}
}
