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
            
            var requestParameters = request.QueryResult.Parameters;
            string issueId = requestParameters.Fields["issue-id"].StringValue;
            return await JiraAPIContext.getIssueDetails(issueId);
            
        }

        public static async Task<string> ChangeStatusOfIssue(WebhookRequest request) {

            var requestParameters = request.QueryResult.Parameters;
            string issueId = requestParameters.Fields["issue-id"].StringValue;
            string newStatusName = requestParameters.Fields["status"].StringValue.ToLower();
            string newStatusId = JiraHelper.StatusNametoCode(newStatusName); 
            var responseString =  await JiraAPIContext.changeIssueStatus(issueId, newStatusId);
            return JiraHelper.GenerateResponseForChangingStatus(responseString, issueId, newStatusName);
            

        }

        public static async Task<string> AddComment(WebhookRequest request) {
            var requestParameters = request.QueryResult.Parameters;
            string issueId = requestParameters.Fields["issue-id"].StringValue;
            string newComment = JiraHelper.ExtractCommentFromParams(requestParameters);
            return await JiraAPIContext.addComment(issueId, newComment); 
            
        }
    }
}
