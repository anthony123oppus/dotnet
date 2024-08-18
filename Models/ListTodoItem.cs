using System.Collections.Generic;
using dotnet.Models;

namespace dotnet.ViewModels
{
    public class TodoViewModel
    {
        public List<TodoItem>? TodoList { get; set; }
        // public TodoItem? Todo { get; set; }
        public TodoItem? Todo { get; set; }
    }
}