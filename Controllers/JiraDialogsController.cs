using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using MeetingApi.Controllers.Helpers;
using MeetingApi.Controllers.Service.JiraService;
using MeetingApi.Controllers.Service.TwilioService;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace MeetingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraDialogsController : ControllerBase
    {
        
        private static readonly JsonParser jsonParser = DialogService.returnNewJsonParser();


        [HttpPost]
        public async Task<ContentResult> DialogActionAsync()
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


            // Add a comment. Params  - issueKey and comment
            if (request.QueryResult.Action == "addIssueComment")
            {
                textToReturn = await JiraService.AddComment(request);
            }

            // Show all todo's from the database
            else if (request.QueryResult.Action == "changeIssueStatus")
            {
                textToReturn = await JiraService.ChangeStatusOfIssue(request);
            }
            else if (request.QueryResult.Action == "getIssueDetails")
            {
                textToReturn = await JiraService.GetIssueDetails(request);
            }

            // 
            else if (request.QueryResult.Action == "getAllAssignedIssues")
            {
                await TwilioService.SendMorningMessagesAsync();
            }
            

            else
            {
                textToReturn = "Your action could not be resolved!";
            }

            string responseJson = DialogService.populateResponse(textToReturn);
            var content = Content(responseJson, "application/json");

            return content;
        }

    }
}