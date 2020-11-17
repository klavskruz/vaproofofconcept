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
            string accountSid = "AC6ca0291c85bac2bf44ec8bd5df46e833";
            string authToken = "60c9c687bd61e9917a929e7da6125e87";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: smsText,
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+447376483667")
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
