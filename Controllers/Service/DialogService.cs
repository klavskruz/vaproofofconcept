using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using MeetingApi.Models.CustomPayload;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Google.Cloud.Dialogflow.V2.Intent.Types;
using static Google.Cloud.Dialogflow.V2.Intent.Types.Message.Types;

namespace MeetingApi.Controllers.Helpers
{
    public class DialogService
    {

        public static JsonParser returnNewJsonParser()
        {
            // A Protobuf JSON parser configured to ignore unknown fields. This makes
            // the action robust against new fields being introduced by Dialogflow.
            return new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        }
        public static string populateResponse(string text)
        {
            var teamsMessage = PayloadHelper.CreateSkypePayload(text);

            // Populate the response
            WebhookResponse response = new WebhookResponse();
            response.FulfillmentMessages.Add(teamsMessage);
            string responseJson = response.ToString();
            return responseJson;
            
        }




    }

}

