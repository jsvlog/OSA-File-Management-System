using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OSAWebAPI.Models;
using OSAWebAPI.Services;

namespace OSAWebAPI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RegionComService _service;

    public HomeController(ILogger<HomeController> logger, RegionComService service)
    {
        _logger = logger;
        _service = service;
    }

    public IActionResult Index()
    {
        var stats = _service.GetStatistics();
        return View(stats);
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
