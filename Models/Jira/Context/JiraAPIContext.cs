using MeetingApi.Models.Jira;
using MeetingApi.Models.Jira.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MeetingApi.Controllers.Service.TwilioService;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using MeetingApi.Models.Jira.IssueShortened;

namespace MeetingApi.Controllers.Service.JiraService
{
    public class JiraAPIContext
    {

        public static async Task<string> getIssueDetails(string issueId)
        {

            using (var httpClient = new HttpClient())
            {

                string urlWithIdInserted = $"https://vapoc.atlassian.net/rest/api/3/issue/{issueId}";

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), ($"{urlWithIdInserted}")))
                {

                    var response = await JiraHelper.GetResponseMessageAsync(httpClient, request);

                    // Check if issue with given ID exists
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (responseString.Contains("Issue does not exist or you do not have permission to see it."))
                    {
                        return "There is no issue with the given ID";
                    }

                    var issueResult = await response.Content.ReadFromJsonAsync<Issue>();
                    string issueSummary = issueResult.fields.summary;
                    string issueStatus = issueResult.fields.status.name;
                    string issueDescription = issueResult.fields.description.content[0].content[0].text;
                    string lastComment = JiraInputValidator.getLastCommentIfExists(issueResult);
                    string issueEstimateOriginal = JiraInputValidator.getOriginalEstimateIfExists(issueResult);
                    string issueEstimateRemaining = JiraInputValidator.getRemainingEstimateIfExists(issueResult);
                    string issueDetails = (
                                            $"Summary: {issueSummary} \n" +
                                            $"Latest Comment: {lastComment} \n" +
                                            $"Status: {issueStatus} \n" +
                                            $"Original Estimate: {issueEstimateOriginal} days\n" +
                                            $"Remaining estimate: {issueEstimateRemaining} days\n" +
                                            $"Description: " + issueDescription
                        );
                    return issueDetails;

                }

            }
        }
        public static async Task<List<IssueShortened>> GetAssignedIssues()
        {

            using (var httpClient = new HttpClient())
            {


                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://vapoc.atlassian.net/rest/api/2/search?jql=assignee=currentuser()"))
                {
                    AssignedIssues userIssues;
                    var response = await JiraHelper.GetResponseMessageAsync(httpClient, request);
                    userIssues = await response.Content.ReadFromJsonAsync<AssignedIssues>();
                    var sortedIssues = IssueService.IssueService.sortIssueListByPriority(userIssues.issues);
                    return sortedIssues;

                }


            }

        }


        public static async Task<string> changeIssueStatus(string issueId, string statusId)
        {

            using (var httpClient = new HttpClient())
            {

                string urlWithIdInserted = $"https://vapoc.atlassian.net/rest/api/3/issue/{issueId}/transitions";
                string jsonBodyContent = $"{{\"transition\":{{\"id\":\"{statusId}\"}}}}";
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), ($"{urlWithIdInserted}")))
                {

                    request.Content = new StringContent(jsonBodyContent);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    var response = await JiraHelper.GetResponseMessageAsync(httpClient, request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    return responseString;

                }

            }
        }

        
        public static async Task<string> addComment(string issueId, string comment)
        {

            using (var httpClient = new HttpClient())
            {
                string bodyWithComment = JiraHelper.ParseCommentIntoBody(comment);
                string urlWithIdInserted = $"https://vapoc.atlassian.net/rest/api/3/issue/{issueId}/comment";

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), ($"{urlWithIdInserted}")))
                {
                    request.Content = new StringContent(bodyWithComment);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    var response = await JiraHelper.GetResponseMessageAsync(httpClient, request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (responseString.Contains("Issue does not exist or you do not have permission to see it."))
                    {
                        return "There is no issue with the given ID";
                    }
                    return "Comment added";

                }

            }
        }


        public static async Task<string> getUserGroup()
        {

            using (var httpClient = new HttpClient())
            {

                string urlWithIdInserted = $"https://vapoc.atlassian.net/rest/api/3/group/member?groupname=Administrators";

                using (var request = new HttpRequestMessage(new HttpMethod("GET"), ($"{urlWithIdInserted}")))
                {
                    var response = await JiraHelper.GetResponseMessageAsync(httpClient, request);
                    return await response.Content.ReadAsStringAsync();

                }

            }
        }



    }
}
