using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ALBERT.Models;
using ALBERT.Repositories;
using System.Configuration;

namespace ALBERT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TableRepository _tableRepository;
    public HomeController(IConfiguration configuration,ILogger<HomeController> logger)
    {
        _logger = logger;
        _tableRepository = new TableRepository(configuration.GetConnectionString("DefaultConnection"));
    }

    public IActionResult Index()
    {
        var tables = _tableRepository.GetAllTables();
        return View(tables);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
