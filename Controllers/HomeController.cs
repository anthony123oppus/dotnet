using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using dotnet.ViewModels;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Reflection.Metadata;

namespace dotnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private string connectionString; // Variable For Database Connection

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        // Start Database Connection Config
        var server = Environment.GetEnvironmentVariable("DB_SERVER");
        var database = Environment.GetEnvironmentVariable("DB_DATABASE");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        // Connection String assign to the connectionString
        connectionString = $"Server={server};Database={database};User={user};Password={password}";
        // End Database Connection Config
    }

    public IActionResult Index()
    {
        var todos = GetAllTodos();

        // start code just checking into the console the data
        if (todos.TodoList != null)
        {
            foreach (var item in todos.TodoList)
            {
                Console.WriteLine($"{item.Id} {item.Name}");
            }
            Console.WriteLine(JsonSerializer.Serialize(todos.TodoList));
        }
        // end code in checking into the console the data

        ViewData["todos"] = todos;
        ViewBag.Message = TempData["message"];

        return View(); // Pass the list to the view
    }

    internal TodoViewModel GetAllTodos()
    {
        // string Server = "localhost";
        // string Database = "dotnet";
        // string User = "root";
        // string Password = "password";

        // var connectionString = $"Server={Server};Database={Database};User={User};Password={Password}";

        var todos = new List<TodoItem>();

        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = "SELECT * FROM todo";

                try
                {
                    using (var reader = tableCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var todo = new TodoItem
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name")
                            };
                            todos.Add(todo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        return new TodoViewModel
        {
            TodoList = todos
        };
    }

    public IActionResult Insert(TodoItem todo)
    {
        // string Server = "localhost";
        // string Database = "dotnet";
        // string User = "root";
        // string Password = "password";

        // var connectionString = $"Server={Server};Database={Database};User={User};Password={Password}";

        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            Console.WriteLine(todo);
            if (todo != null)
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"INSERT INTO todo (name) VALUES (@name)";
                    tableCmd.Parameters.AddWithValue("@name", todo.Name);
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                        TempData["message"] = "Todo Added Succesfully";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        TempData["message"] = "Todo Added Failed";

                    }
                }
            }
            else
            {
                TempData["message"] = "Please Enter Todo";
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
