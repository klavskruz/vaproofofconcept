using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingApi.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Google.Protobuf;
using MeetingApi.Controllers.Helpers;
using System.IO;
using MeetingApi.Models.Jira;
using System.Net.Http.Json;
using MeetingApi.Controllers.Service.JiraService;
using MeetingApi.Models.Jira.Project;
using MeetingApi.Controllers.Service.TwilioService;

namespace MeetingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioTest : ControllerBase
    {
        JiraAPIContext _context;
        private static readonly JsonParser jsonParser = DialogService.returnNewJsonParser();

        
        // GET: api/Meetings


        [HttpPost]
        public async Task<string> JiraFunctionAsync()
        {
            await TwilioService.SendMorningMessagesAsync();
            return "Morning messages sent";
            
        }

        [HttpGet]
        public async Task<string> GetIssueById()
        {
           return await JiraAPIContext.getIssueDetails("VAPOC-1");
            }

        }
    }




