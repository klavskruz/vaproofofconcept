using Google.Cloud.Dialogflow.V2;
using MeetingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingApi.Controllers.Helpers
{
    public class TodoService
    {
        public static async Task<string> newTodoItem(WebhookRequest request, TodoContext _context) {
            //Parse the intent params
            string textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
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
            return textToReturn;
        }
        public static async Task<string> showAllTodos(TodoContext _context)
        {
            string textToReturn = "";
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
            return textToReturn;
        }

        public static async Task<string> deleteTodoGivenDescription(WebhookRequest request, TodoContext _context)
        {
            string textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
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
            return textToReturn;
        }
        public static async Task<string> showTodosDueBy(WebhookRequest request, TodoContext _context) {
            string textToReturn = "";
            var requestParameters = request.QueryResult.Parameters;
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
            return textToReturn;
        }

        public static async Task<string> showAllUrgentTodos(TodoContext _context)
        {
            string textToReturn = "";
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
            return textToReturn;
        }
        
    }
}
