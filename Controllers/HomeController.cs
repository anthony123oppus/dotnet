using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using MySql.Data.MySqlClient;

namespace dotnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(); // Pass the list to the view
    }
    public IActionResult Insert(TodoItem todo)
    {
        string Server = "localhost";
        string Database = "dotnet";
        string User = "root";
        string Password = "password";

        var connectionString = $"Server={Server};Database={Database};User={User};Password={Password}";

        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            using (var tableCmd = con.CreateCommand())
            {
                con.Open();
                tableCmd.CommandText = $"INSERT INTO todo (name) VALUES (@name)";
                tableCmd.Parameters.AddWithValue("@name", todo.Name);
                try
                {
                    tableCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
