using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Models.CustomPayload
{

    public class DialogCard
    {
        public string title { get; set; }
        public string imageUri { get; set; }
        public CardButton[] buttons { get; set; }
    }

}
