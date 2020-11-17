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
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("EMAIL:API KEY"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                    var response = await httpClient.SendAsync(request);
                    await response.Content.ReadAsStringAsync();
                    var issueResult = await response.Content.ReadFromJsonAsync<Issue>();
                    string issueSummary = issueResult.fields.summary;
                    int lastCommentIndex = issueResult.fields.comment.comments.Length-1;
                    string lastComment = issueResult.fields.comment.comments[lastCommentIndex].body.content[0].content[0].text;
                    string issueStatus = issueResult.fields.status.name;
                    string issueEstimateOriginal = (Int32.Parse(issueResult.fields.timeoriginalestimate.ToString()) / 28000).ToString();
                    string issueEstimateRemaining = (Int32.Parse(issueResult.fields.timeestimate.ToString()) / 28000).ToString();
                    string issueDescription = issueResult.fields.description.content[0].content[0].text;
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
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("klavs.kruzins@gmail.com:fpd7JDMRYwa4aRmRPH8uD0CB"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                    var response = await httpClient.SendAsync(request);
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
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("klavs.kruzins@gmail.com:fpd7JDMRYwa4aRmRPH8uD0CB"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                    request.Content = new StringContent(jsonBodyContent);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    var response = await httpClient.SendAsync(request);
                    return await response.Content.ReadAsStringAsync();

                }

            }
        }


        public static async Task<string> addComment(string issueId, string comment)
        {

            using (var httpClient = new HttpClient())
            {
                string bodyWithComment = $"{{\n  \"visibility\": {{\n    \"type\": \"role\",\n    \"value\": \"Administrators\"\n  }},\n  \"body\": {{\n    \"type\": \"doc\",\n    \"version\": 1,\n    \"content\": [\n      {{\n        \"type\": \"paragraph\",\n        \"content\": [\n          {{\n            \"text\": \"{comment}\",\n            \"type\": \"text\"\n          }}\n        ]\n      }}\n    ]\n  }}\n}}";
                string urlWithIdInserted = $"https://vapoc.atlassian.net/rest/api/3/issue/{issueId}/comment";
                Console.WriteLine(comment);
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), ($"{urlWithIdInserted}")))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("klavs.kruzins@gmail.com:fpd7JDMRYwa4aRmRPH8uD0CB"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                    request.Content = new StringContent(bodyWithComment);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    var response = await httpClient.SendAsync(request);
                    return await response.Content.ReadAsStringAsync();

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
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("klavs.kruzins@gmail.com:fpd7JDMRYwa4aRmRPH8uD0CB"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                    var response = await httpClient.SendAsync(request);
                    return await response.Content.ReadAsStringAsync();

                }

            }
        }



    }
}
