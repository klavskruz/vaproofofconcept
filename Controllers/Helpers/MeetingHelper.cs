using Google.Cloud.Dialogflow.V2;
using Google.Protobuf.WellKnownTypes;
using MeetingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Controllers.Helpers
{
    public class MeetingHelper
    {
        static string textToReturn;
        public static async Task<string> AddMeetingToDatabase(WebhookRequest request, MeetingContext _context)
        {
            textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
            string person = requestParameters.Fields["given-name"].StringValue;
            string location = requestParameters.Fields["Place"].StringValue;
            var timeStruct = requestParameters.Fields["date-time"].StructValue;
            string structString = timeStruct.ToString();
            string time = structString.Split('"')[3];

            Meeting meeting = new Meeting
            {
                Location = location,
                Person = person,
                Time = DateTimeOffset.Parse(time).DateTime

            };
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            textToReturn = "Meeting added";
            return textToReturn;
        }


        public static async Task<string> ShowAllMeetings(WebhookRequest request, MeetingContext _context) {
            textToReturn = "";
            var allMeetings = await _context.Meetings.ToListAsync();
            if (allMeetings.Any())
            {
                foreach (Meeting meeting in allMeetings)
                {
                    textToReturn += $"You're meeting, {meeting.Person} on {meeting.Time.ToShortDateString()} at {meeting.Time.ToShortTimeString()} in the {meeting.Location}." + System.Environment.NewLine;
                }
            }
            else
            {
                textToReturn = "There are no meetings for you!";
            }
            return textToReturn;
        }

        public static async Task<string> DeleteMeetingFromDatabase(Struct requestParameters, MeetingContext _context) {
            textToReturn = "";
            var timeStruct = requestParameters.Fields["date-time"].StructValue;
            string structString = timeStruct.ToString();
            string time = structString.Split('"')[3];
            var meeting = await _context.Meetings.FirstOrDefaultAsync(m => m.Time == DateTimeOffset.Parse(time).DateTime);
            Console.WriteLine(time);

            if (meeting == null)
            {
                textToReturn = "There is no such meeting";
            }
            else
            {
                _context.Meetings.Remove(meeting);
                await _context.SaveChangesAsync();

                textToReturn = "Deleted!";
            }
            return textToReturn;
        }

        public static async Task<string> FindMeetingsFromDatabase(Struct requestParameters, MeetingContext _context) {
            textToReturn = "";
            string person = requestParameters.Fields["given-name"].StringValue;
            var meetings = await _context.Meetings.ToListAsync();
            var filteredMeetings = meetings.Where(m => m.Person == person);

            if (meetings.Any())
            {
                foreach (Meeting meeting in filteredMeetings)
                {
                    textToReturn += $"At {meeting.Time} in the {meeting.Location}" + System.Environment.NewLine;
                }
            }
            else
            {
                textToReturn = $"You have no meetings scheudled with {person}";
            }
            return textToReturn;
        }
    }
}
