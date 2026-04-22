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

    public IActionResult Index(int? year, string? direction, string? search, int? page, string? sort)
    {
        int pageSize = 25;
        int currentPage = page ?? 1;

        var allRecords = _service.Filter(year, null, search);

        if (!string.IsNullOrEmpty(direction) && direction != "All")
        {
            allRecords = allRecords.Where(r => r.Direction == direction).ToList();
        }

        // Sort: default is Control Number latest at top; toggle on click
        bool sortByControlDesc = string.IsNullOrEmpty(sort) || sort != "controlAsc";
        if (sortByControlDesc)
        {
            allRecords = SortByControlNumberDesc(allRecords);
        }
        else
        {
            allRecords = SortByControlNumberAsc(allRecords);
        }

        int totalRecords = allRecords.Count;
        int totalPages = totalRecords > 0 ? (int)Math.Ceiling((double)totalRecords / pageSize) : 1;

        var pagedRecords = allRecords
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = currentPage;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalRecords = totalRecords;
        ViewBag.PageSize = pageSize;
        ViewBag.SelectedYear = year;
        ViewBag.SelectedDirection = direction ?? "All";
        ViewBag.SearchTerm = search;
        ViewBag.CurrentSort = sortByControlDesc ? "controlDesc" : "controlAsc";
        ViewBag.Years = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).Reverse().ToList();

        return View(pagedRecords);
    }

    private List<RegionComModel> SortByControlNumberDesc(List<RegionComModel> list)
    {
        return list
            .OrderByDescending(item => GetControlYear(item.RefNumber))
            .ThenByDescending(item => GetControlNumber(item.RefNumber))
            .ThenByDescending(item => item.RefNumber ?? string.Empty)
            .ToList();
    }

    private List<RegionComModel> SortByControlNumberAsc(List<RegionComModel> list)
    {
        return list
            .OrderBy(item => GetControlYear(item.RefNumber))
            .ThenBy(item => GetControlNumber(item.RefNumber))
            .ThenBy(item => item.RefNumber ?? string.Empty)
            .ToList();
    }

    private static int GetControlYear(string? refNumber)
    {
        if (string.IsNullOrWhiteSpace(refNumber))
            return 0;
        var parts = refNumber.Split('-');
        if (parts.Length >= 2 && int.TryParse(parts[1], out int year))
            return year;
        return 0;
    }

    private static int GetControlNumber(string? refNumber)
    {
        if (string.IsNullOrWhiteSpace(refNumber))
            return 0;
        var parts = refNumber.Split('-');
        if (parts.Length >= 3 && int.TryParse(parts[2], out int num))
            return num;
        return 0;
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