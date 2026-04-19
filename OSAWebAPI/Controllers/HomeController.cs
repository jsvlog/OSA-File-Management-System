using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OSAWebAPI.Models;
using OSAWebAPI.Services;

namespace OSAWebAPI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RegionComService _regionComService;
    private readonly InventoryService _inventoryService;
    private readonly MonitoringService _monitoringService;

    public HomeController(
        ILogger<HomeController> logger,
        RegionComService regionComService,
        InventoryService inventoryService,
        MonitoringService monitoringService)
    {
        _logger = logger;
        _regionComService = regionComService;
        _inventoryService = inventoryService;
        _monitoringService = monitoringService;
    }

    public IActionResult Index()
    {
        var stats = _regionComService.GetStatistics();
        try
        {
            var inventoryCount = _inventoryService.GetAll().Count;
            ViewBag.InventoryCount = inventoryCount;
            ViewBag.InventoryYearRange = _inventoryService.GetYearRange();
        }
        catch
        {
            ViewBag.InventoryCount = 0;
            ViewBag.InventoryYearRange = "No data";
        }
        return View(stats);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}