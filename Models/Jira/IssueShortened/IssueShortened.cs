using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models.Jira.IssueShortened
{

    public class IssueShortened
    {
        public string expand { get; set; }
        public Fields fields { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string self { get; set; }
    }

    public class Fields
    {
        public object aggregatetimeestimate { get; set; }
        public object aggregatetimeoriginalestimate { get; set; }
        public object aggregatetimespent { get; set; }
        public Assignee assignee { get; set; }
        public object[] components { get; set; }
        
        public string description { get; set; }
        public object duedate { get; set; }
        public object environment { get; set; }
        public object[] fixVersions { get; set; }
        public object[] issuelinks { get; set; }
        public Issuetype issuetype { get; set; }
        public object[] labels { get; set; }
        public Priority priority { get; set; }
        public Progress progress { get; set; }
        public Project project { get; set; }
        public Reporter reporter { get; set; }
        public Status status { get; set; }
        public Votes votes { get; set; }
        public Watches watches { get; set; }
        public int workratio { get; set; }
        public string summary { get; set; }

    }


    public class Assignee
    {
        public string accountId { get; set; }
        public string accountType { get; set; }
        public bool active { get; set; }
        public string displayName { get; set; }
        public string emailAddress { get; set; }
        public string self { get; set; }
        public string timeZone { get; set; }
    }



    public class Creator
    {
        public string accountId { get; set; }
        public string accountType { get; set; }
        public bool active { get; set; }
        public string displayName { get; set; }
        public string emailAddress { get; set; }
        public string self { get; set; }
        public string timeZone { get; set; }
    }



    public class Issuetype
    {
        public int avatarId { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string self { get; set; }
        public bool subtask { get; set; }
    }

    public class Priority
    {
        public string iconUrl { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string self { get; set; }
    }

    public class Progress
    {
        public int progress { get; set; }
        public int total { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string projectTypeKey { get; set; }
        public string self { get; set; }
        public bool simplified { get; set; }
    }


    public class Reporter
    {
        public string accountId { get; set; }
        public string accountType { get; set; }
        public bool active { get; set; }
        public string displayName { get; set; }
        public string emailAddress { get; set; }
        public string self { get; set; }
        public string timeZone { get; set; }
    }


    public class Status
    {
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string self { get; set; }
        public Statuscategory statusCategory { get; set; }
    }

    public class Statuscategory
    {
        public string colorName { get; set; }
        public int id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string self { get; set; }
    }

    public class Votes
    {
        public bool hasVoted { get; set; }
        public string self { get; set; }
        public int votes { get; set; }
    }

    public class Watches
    {
        public bool isWatching { get; set; }
        public string self { get; set; }
        public int watchCount { get; set; }
    }

}
