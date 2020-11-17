using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models.Jira.Project
{

    public class AssignedIssues
    {
        public string expand { get; set; }
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public IssueShortened.IssueShortened[] issues { get; set; }
    }

}
