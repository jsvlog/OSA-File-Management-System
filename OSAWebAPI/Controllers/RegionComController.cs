using Microsoft.AspNetCore.Mvc;
using OSAWebAPI.Models;
using OSAWebAPI.Services;

namespace OSAWebAPI.Controllers;

public class RegionComController : Controller
{
    private readonly RegionComService _service;

    public RegionComController(RegionComService service)
    {
        _service = service;
    }

    public IActionResult Index(int? year, string? type, string? direction, string? search, int? page)
    {
        int pageSize = 25;
        int currentPage = page ?? 1;

        var allRecords = _service.Filter(year, null, search);

        if (!string.IsNullOrEmpty(direction) && direction != "All")
        {
            allRecords = allRecords.Where(r => r.Direction == direction).ToList();
        }

        if (!string.IsNullOrEmpty(type))
        {
            allRecords = allRecords.Where(r => r.TypeOfDocs == type).ToList();
        }

        int totalRecords = allRecords.Count;
        int totalPages = totalRecords > 0 ? (int)Math.Ceiling((double)totalRecords / pageSize) : 1;

        var pagedRecords = allRecords
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var allForTypes = _service.GetAll();
        var types = allForTypes.Select(r => r.TypeOfDocs).Distinct().Where(t => !string.IsNullOrEmpty(t)).OrderBy(t => t).ToList();

        ViewBag.CurrentPage = currentPage;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalRecords = totalRecords;
        ViewBag.PageSize = pageSize;
        ViewBag.SelectedYear = year;
        ViewBag.SelectedType = type;
        ViewBag.SelectedDirection = direction ?? "All";
        ViewBag.SearchTerm = search;
        ViewBag.Years = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).Reverse().ToList();
        ViewBag.Types = types;

        return View(pagedRecords);
    }

    public IActionResult Details(int id)
    {
        var record = _service.GetById(id);
        if (record == null)
            return NotFound();

        var relatedDocs = _service.GetRelatedByTrackingCode(record.TrackingCode, id);
        ViewBag.RelatedDocs = relatedDocs;

        return View(record);
    }

    [HttpGet]
    public IActionResult GetStats()
    {
        var stats = _service.GetStatistics();
        return Json(stats);
    }
}