using MeetingApi.Models.Jira.Project;
using System;


namespace MeetingApi.Controllers.Service.JiraService
{
    public class JiraInputValidator
    {
        public static string getLastCommentIfExists(Issue issueResult) {
            try
            {
                int lastCommentIndex = issueResult.fields.comment.comments.Length - 1;
                return issueResult.fields.comment.comments[lastCommentIndex].body.content[0].content[0].text ?? "No comments on issue";
            }
            catch (Exception e) {
                
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return "No comments in this issue";
            }
        }

        public static string getOriginalEstimateIfExists(Issue issueResult) {
            try {
                return (Int32.Parse(issueResult.fields.timeoriginalestimate.ToString()) / 28000).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return "No original estimate";
            }
        }

        public static string getRemainingEstimateIfExists(Issue issueResult) {
            try
            {
                return (Int32.Parse(issueResult.fields.timeestimate.ToString()) / 28000).ToString();
            }
            catch (Exception e) {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return "No remaining estimate";
            }
        }
       
    }
}
