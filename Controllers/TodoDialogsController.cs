using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using MeetingApi.Controllers.Helpers;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace MeetingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoDialogsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoDialogsController(TodoContext context)
        {
            _context = context;
        }

        private static readonly JsonParser jsonParser = DialogService.returnNewJsonParser();
      

        [HttpPost]
        public async Task<ContentResult> DialogActionAsync()
        {
            // Parse the body of the request using the Protobuf JSON parser,
            // *not* Json.NET.
            string textToReturn = "";
            string requestJson;
            using (TextReader reader = new StreamReader(Request.Body))
            {
                requestJson = await reader.ReadToEndAsync();
            }

            WebhookRequest request;

            request = jsonParser.Parse<WebhookRequest>(requestJson);
           

            // Add a todo into a database
            if (request.QueryResult.Action == "addTodo")
            {
                textToReturn = await TodoService.newTodoItem(request, _context);
            }

            // Show all todo's from the database
            else if (request.QueryResult.Action == "showAll")
            {
                textToReturn = await TodoService.showAllTodos(_context);
            }

            // Delete todo given it's description
            else if (request.QueryResult.Action == "deleteTodo")
            {
                textToReturn = await TodoService.deleteTodoGivenDescription(request, _context);
            }
            // Find all todo's due by a certain date
            else if (request.QueryResult.Action == "dueByDate")
            {
                textToReturn = await TodoService.showTodosDueBy(request,_context);    
            }
            // Show all todo's which are urgent
            else if (request.QueryResult.Action == "showUrgent")
            {
                textToReturn = await TodoService.showAllUrgentTodos(_context);
              
            }

            else
            {
                textToReturn = "Your action could not be resolved!";
            }

            string responseJson = DialogService.populateResponse(textToReturn);
            var content = Content(responseJson, "application/json");

            return content;
        }

    }
}