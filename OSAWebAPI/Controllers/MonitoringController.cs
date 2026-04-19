using Microsoft.AspNetCore.Mvc;
using OSAWebAPI.Services;
using OSAWebAPI.Models;

namespace OSAWebAPI.Controllers;

public class MonitoringController : Controller
{
    private readonly MonitoringService _monitoringService;

    public MonitoringController(MonitoringService monitoringService)
    {
        _monitoringService = monitoringService;
    }

    public IActionResult Index()
    {
        var docTypes = _monitoringService.GetDocTypes();
        return View(docTypes);
    }

    public IActionResult Status(string docType, int? year)
    {
        if (string.IsNullOrEmpty(docType))
        {
            return RedirectToAction("Index");
        }

        int selectedYear = year ?? DateTime.Now.Year;
        var grid = _monitoringService.GetStatusGrid(docType, selectedYear);
        var municipalities = _monitoringService.GetMunicipalities();
        var years = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).Reverse().ToList();

        ViewBag.DocumentType = docType;
        ViewBag.SelectedYear = selectedYear;
        ViewBag.Years = years;
        ViewBag.Municipalities = municipalities;

        return View(grid);
    }

    [HttpGet]
    public IActionResult GetStatusData(string docType, int year)
    {
        var grid = _monitoringService.GetStatusGrid(docType, year);
        return Json(grid);
    }
}