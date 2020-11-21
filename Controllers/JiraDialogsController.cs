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


            // Add a comment
            if (request.QueryResult.Action == "addIssueComment")
            {
                textToReturn = await JiraService.AddComment(request);
            }

            // Change issue status
            else if (request.QueryResult.Action == "changeIssueStatus")
            {
                textToReturn = await JiraService.ChangeStatusOfIssue(request);
            }
            // Get issue details
            else if (request.QueryResult.Action == "getIssueDetails")
            {
                textToReturn = await JiraService.GetIssueDetails(request);
            }

            // Get all the issues asssigned to the user
            else if (request.QueryResult.Action == "getAllAssignedIssues")
            {
                textToReturn = JiraHelper.CreateAssignedIssueTable(await JiraAPIContext.GetAssignedIssues());
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