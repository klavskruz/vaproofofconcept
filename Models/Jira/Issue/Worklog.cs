namespace MeetingApi.Models.Jira.Project
{
    public class Worklog
    {
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public object[] worklogs { get; set; }
    }



}
