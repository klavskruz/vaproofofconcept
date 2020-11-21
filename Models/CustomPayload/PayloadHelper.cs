using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Google.Cloud.Dialogflow.V2.Intent.Types;

namespace MeetingApi.Models.CustomPayload
{
    public class PayloadHelper
    {
        public static Message CreateSkypePayload(string textToReturn)
        {
            var skypeText = Value.ForStruct(new Struct
            {
                Fields ={
             ["text"] = Value.ForString(textToReturn),

    }
            });
            var skypePayload = new Struct
            {
                Fields = {
                    ["skype"] = skypeText
                }
            };

            Message msg = new Message
            {
                Payload = skypePayload,
            };

            return msg;
        }

    }
}
