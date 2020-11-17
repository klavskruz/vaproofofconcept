using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models.Jira.Issue
{

    public class CommentObject
    {
        public string self { get; set; }
        public string id { get; set; }
        public Author author { get; set; }
        public Body body { get; set; }
        public Updateauthor updateAuthor { get; set; }
        //public DateTime created { get; set; }
       // public DateTime updated { get; set; }
        public Visibility visibility { get; set; }
        public bool jsdPublic { get; set; }
    }

    public class Author
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public string emailAddress { get; set; }
        public Avatarurls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Avatarurls
    {
        public string _48x48 { get; set; }
        public string _24x24 { get; set; }
        public string _16x16 { get; set; }
        public string _32x32 { get; set; }
    }

    public class Body
    {
        public string type { get; set; }
        public int version { get; set; }
        public CommentObjectContent[] content { get; set; }
    }

    public class CommentObjectContent
    {
        public string type { get; set; }
        public CommentObjectContent1[] content { get; set; }
    }

    public class CommentObjectContent1
    {
        public string text { get; set; }
        public string type { get; set; }
    }

    public class Updateauthor
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public string emailAddress { get; set; }
        public Avatarurls1 avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }
    }

    public class Avatarurls1
    {
        public string _48x48 { get; set; }
        public string _24x24 { get; set; }
        public string _16x16 { get; set; }
        public string _32x32 { get; set; }
    }

    public class Visibility
    {
        public string type { get; set; }
        public string value { get; set; }
    }

}
