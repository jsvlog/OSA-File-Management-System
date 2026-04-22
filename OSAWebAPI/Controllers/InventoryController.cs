using Microsoft.AspNetCore.Mvc;
using OSAWebAPI.Services;
using OSAWebAPI.Models;

namespace OSAWebAPI.Controllers;

public class InventoryController : Controller
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public IActionResult Index(int? year, string? search)
    {
        var records = _inventoryService.Filter(year, null, search);
        var allYears = _inventoryService.GetDistinctYears();

        if (allYears.Count == 0)
        {
            allYears = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).Reverse().ToList();
        }

        ViewBag.SelectedYear = year;
        ViewBag.SearchTerm = search;
        ViewBag.Years = allYears;

        return View(records);
    }

    public IActionResult Details(int id)
    {
        var record = _inventoryService.GetById(id);
        if (record == null)
            return NotFound();

        return View(record);
    }
}