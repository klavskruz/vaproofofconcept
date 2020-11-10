using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using MeetingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
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
        // A Protobuf JSON parser configured to ignore unknown fields. This makes
        // the action robust against new fields being introduced by Dialogflow.
        private static readonly JsonParser jsonParser =
        new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

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
            var requestParameters = request.QueryResult.Parameters;

            // ADD TO DO ITEM
            if (request.QueryResult.Action == "addTodo")
            {
                //Parse the intent params
                
                string dueDateString = requestParameters.Fields["date-time"].StringValue;
                DateTime dueDate = DateTimeOffset.Parse(dueDateString).UtcDateTime;
                string decription = requestParameters.Fields["description"].StringValue;
                string isUrgent = requestParameters.Fields["is-urgent"].StringValue;
               

                Todo todo = new Todo
                {
                    DueDate = dueDate.Date,
                    Description = decription,
                    Urgent = isUrgent,
                };

                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();
                textToReturn = "To do task added!";



            }
            // DISPLAY ALL TO DO's 
            else if (request.QueryResult.Action == "showAll")
            {
                var allTodos = await _context.Todos.ToListAsync();
                if (allTodos.Any())
                {
                    textToReturn += "You have to do the following things: " + System.Environment.NewLine;
                    foreach (Todo todo in allTodos)
                    {
                        textToReturn += $"{todo.Description} by {todo.DueDate.ToShortDateString()}. High priority : {todo.Urgent}" + System.Environment.NewLine;
                    }
                }
                else
                {
                    textToReturn = "You have done it all!";
                }
            }

            // DELETE TO DO ITEM PROVIDED THE DESCRIPTION
            else if (request.QueryResult.Action == "deleteTodo")
            {
                
                string description = requestParameters.Fields["description"].StringValue.ToLower();
                var todo = _context.Todos.FirstOrDefault(t => t.Description.ToLower().Equals(description));
                if (todo == null)
                {
                    textToReturn = "No such task!";
                }
                else
                {
                    _context.Todos.Remove(todo);
                    await _context.SaveChangesAsync();

                    textToReturn = "Removed!";
                }



            }
            // FIND TODO's WHICH ARE DUE BY A CERTAIN DATE
            else if (request.QueryResult.Action == "dueByDate")
            {
                
                string dueDateString = requestParameters.Fields["date-time"].StringValue;
                DateTime dueDate = DateTimeOffset.Parse(dueDateString).UtcDateTime;
                var todos = await _context.Todos.ToListAsync();
                var filteredTodos = todos.Where(t => t.DueDate.ToShortDateString().Equals(dueDate.ToShortDateString()));

                if (filteredTodos.Any())
                {
                    textToReturn += $"By {dueDate} you have to :" + System.Environment.NewLine;
                    foreach (Todo todo in filteredTodos)
                    {
                        textToReturn += $"{todo.Description}. " + System.Environment.NewLine;
                    }
                }
                else
                {
                    textToReturn = $"You are all clear.";
                }
            }

            else if (request.QueryResult.Action == "showUrgent")
            {

               
                var todos = await _context.Todos.ToListAsync();
                var filteredTodos = todos.Where(t => t.Urgent.Equals("true"));

                if (filteredTodos.Any())
                {
                    textToReturn += $"Your high priority tasks are :" + System.Environment.NewLine;
                    foreach (Todo todo in filteredTodos)
                    {
                        textToReturn += $"{todo.Description} by {todo.DueDate.ToShortDateString()}. " + System.Environment.NewLine;
                    }
                }
                else
                {
                    textToReturn = $"No high priority tasks.";
                }
            }



            else
            {
                textToReturn = "Something has gone terribly wrong mate!";

            }

            // Populate the response
            WebhookResponse response = new WebhookResponse
            {
                FulfillmentText = textToReturn
            };
            // Ask Protobuf to format the JSON to return.
           
            string responseJson = response.ToString();
            return Content(responseJson, "application/json");
        }

    }
}