using MeetingApi.Models.Jira.IssueShortened;
using MeetingApi.Models.Jira.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Controllers.Service.IssueService
{
    public class IssueService
    {
        public static List<IssueShortened> sortIssueListByPriority(IssueShortened[] unsortedArray) {

            List<IssueShortened> lowestPriorityIssues = new List<IssueShortened>();
            List<IssueShortened> lowPriorityIssues = new List<IssueShortened>();
            List<IssueShortened> midPriorityIssues = new List<IssueShortened>();
            List<IssueShortened> highPriorityIssues = new List<IssueShortened>();
            List<IssueShortened> highestPriorityIssues = new List<IssueShortened>();
            List<IssueShortened> sortedIssues = new List<IssueShortened>();

            foreach (IssueShortened unsortedIssue in unsortedArray)
            {
                Console.WriteLine("Pritority : " + unsortedIssue.fields.priority.name.ToLower());
                switch (unsortedIssue.fields.priority.name.ToLower())
                {
                    case "lowest":
                        lowestPriorityIssues.Add(unsortedIssue);
                        break;
                    case "low":
                        lowPriorityIssues.Add(unsortedIssue);
                        break;
                    case "medium":
                        midPriorityIssues.Add(unsortedIssue);
                        break;
                    case "high":
                        highPriorityIssues.Add(unsortedIssue);
                        break;
                    case "highest":
                        highestPriorityIssues.Add(unsortedIssue);
                        break;
                }
            }
            sortedIssues.AddRange(lowestPriorityIssues);
            sortedIssues.AddRange(lowPriorityIssues);
            sortedIssues.AddRange(midPriorityIssues);
            sortedIssues.AddRange(highPriorityIssues);
            sortedIssues.AddRange(highestPriorityIssues);

           
            return sortedIssues;


        }
    }
}
