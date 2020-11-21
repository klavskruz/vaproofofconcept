using Google.Protobuf.WellKnownTypes;
using MeetingApi.Extras.TableParser;
using MeetingApi.Models.Jira.IssueShortened;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MeetingApi.Controllers.Service.JiraService
{
    public class JiraHelper
    {
        public static string StatusNametoCode(string statusName) {
            switch (statusName)
            {
                case "backlog":
                    return "11";
                
                case "selected":
                    return "21";
                
                case "in progress":
                    return "31";
                
                case "done":
                    return "41";

                default:
                    return "99";

            }
        }

        public static string GenerateResponseForChangingStatus(string responseString, string issueId, string newStatusName ) {
            if (responseString.Contains("Issue does not exist or you do not have permission to see it."))
            {
                return "There is no issue with the given ID";
            }
            return $"The issue {issueId} has been moved to : {newStatusName}";
            
        }

        public static string ExtractCommentFromParams(Struct requestParameters) {
            return requestParameters.Fields["comment"].StringValue.Substring(1, requestParameters.Fields["comment"].StringValue.Length - 2);
        }

        public static string ParseCommentIntoBody(string comment) {
            return $"{{\n  \"visibility\": {{\n    \"type\": \"role\",\n    \"value\": \"Administrators\"\n  }},\n  \"body\": {{\n    \"type\": \"doc\",\n    \"version\": 1,\n    \"content\": [\n      {{\n        \"type\": \"paragraph\",\n        \"content\": [\n          {{\n            \"text\": \"{comment}\",\n            \"type\": \"text\"\n          }}\n        ]\n      }}\n    ]\n  }}\n}}";
        }

        public static async Task<HttpResponseMessage> GetResponseMessageAsync(HttpClient httpClient, HttpRequestMessage request) {

            AuthorizeJira(request);
            var response = await httpClient.SendAsync(request);
            return response;
        }

        public static void AuthorizeJira(HttpRequestMessage request) {
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes());
            request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
        }

        public static string CreateAssignedIssueTable(List<IssueShortened> assignedIssueList) {
            
                DataTable dt = new DataTable();
                dt.Columns.Add("Id");
                dt.Columns.Add("Key");
                dt.Columns.Add("Summary");
                dt.Columns.Add("Status");
                dt.Columns.Add("Priority");
                
            foreach (var item in assignedIssueList)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = item.id;
                    dr["Key"] = item.key;
                    dr["Summary"] = item.fields.summary;
                    dr["Status"] = item.fields.status.name;
                    dr["Priority"] = item.fields.priority.name;

                dt.Rows.Add(dr);
                }

                return TableParser.ConvertToHtml(dt);
            

        }

      
    }
}
