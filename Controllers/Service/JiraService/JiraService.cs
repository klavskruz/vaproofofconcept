using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetingApi.Controllers.Service.JiraService;


namespace MeetingApi.Controllers.Service.JiraService
{
    public class JiraService
    {
        public static async Task<string> GetIssueDetails (WebhookRequest request)
        {
            string textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
            string issueId = requestParameters.Fields["issue-id"].StringValue;
            textToReturn = await JiraAPIContext.getIssueDetails(issueId);
            return textToReturn;
        }

        public static async Task<string> ChangeStatusOfIssue(WebhookRequest request) {

            string textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
            string issueId = requestParameters.Fields["issue-id"].StringValue;
            string newStatusName = requestParameters.Fields["status"].StringValue.ToLower();
            string newStatusId = "";
            switch (newStatusName) {
                case "backlog":
                    newStatusId = "11";
                    break;
                case "selected":
                    newStatusId = "21";
                    break;
                case "in progress":
                    newStatusId = "31";
                    break;
                case "done":
                    newStatusId = "41";
                    break;
              
            }
            
            await JiraAPIContext.changeIssueStatus(issueId, newStatusId);
            textToReturn = $"The issue {issueId} has been moved to : {newStatusName}";
            return textToReturn;

        }

        public static async Task<string> AddComment(WebhookRequest request) {
            string textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
            string issueId = requestParameters.Fields["issue-id"].StringValue;
            string newComment = requestParameters.Fields["comment"].StringValue.Substring(1, requestParameters.Fields["comment"].StringValue.Length-2);
            await JiraAPIContext.addComment(issueId, newComment);
            textToReturn = "Comment added!";
            return textToReturn;
        }
    }
}
