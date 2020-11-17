using MeetingApi.Controllers.Service.JiraService;
using MeetingApi.Models.Jira.IssueShortened;
using MeetingApi.Models.Jira.Project;
using System;
using System.Collections.Generic;
using Twilio;
using Twilio.Rest.Api.V2010.Account;



namespace MeetingApi.Controllers.Service.TwilioService
{
    public class TwilioService
    {
        public static void SendMessage(string smsText)
        {
            // Find your Account Sid and Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            string accountSid = "sid";
            string authToken = "auth token";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: smsText,
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:number")
            );

            Console.WriteLine(message.Sid);
        }

        public static async System.Threading.Tasks.Task SendMorningMessagesAsync() {
           var issues = await JiraAPIContext.GetAssignedIssues();
           foreach (IssueShortened userIssue in issues)
           {
             string smsText = "Issue id: " + userIssue.id + "\n Summary: " + userIssue.fields.summary + "\n Priority: " + userIssue.fields.priority.name + "\n Status: " + userIssue.fields.status.name;
             SendMessage(smsText);
         }
        }
    }
}
