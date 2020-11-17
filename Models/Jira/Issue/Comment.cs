using MeetingApi.Models.Jira.Issue;

namespace MeetingApi.Models.Jira.Project
{
    public class Comment
    {
        public CommentObject[] comments { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public int startAt { get; set; }
    }



}
