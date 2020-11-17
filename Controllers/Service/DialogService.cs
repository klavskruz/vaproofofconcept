using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Controllers.Helpers
{
    public class DialogService
    {
        
        public static JsonParser returnNewJsonParser() {
            // A Protobuf JSON parser configured to ignore unknown fields. This makes
            // the action robust against new fields being introduced by Dialogflow.
            return new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        }
        public static string populateResponse(string text)
        {

            // Populate the response
            WebhookResponse response = new WebhookResponse
            {
                FulfillmentText = text
            };
            string responseJson = response.ToString();
            return responseJson;
        }




    }

    }

