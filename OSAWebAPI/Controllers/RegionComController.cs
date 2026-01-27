using Microsoft.AspNetCore.Mvc;
using OSAWebAPI.Models;
using OSAWebAPI.Services;

namespace OSAWebAPI.Controllers
{
    public class RegionComController : Controller
    {
        private readonly RegionComService _service;
        
        public RegionComController(RegionComService service)
        {
            _service = service;
        }
        
        public IActionResult Index(int? year, string? type, string? search)
        {
            var records = _service.Filter(year, type, search);
            var stats = _service.GetStatistics();
            
            ViewBag.SelectedYear = year;
            ViewBag.SelectedType = type;
            ViewBag.SearchTerm = search;
            ViewBag.Stats = stats;
            ViewBag.Years = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).Reverse().ToList();
            ViewBag.Types = records.Select(r => r.TypeOfDocs).Distinct().Where(t => !string.IsNullOrEmpty(t)).OrderBy(t => t).ToList();
            
            return View(records);
        }
        
        public IActionResult Details(int id)
        {
            var record = _service.GetById(id);
            if (record == null)
                return NotFound();
            
            return View(record);
        }
        
        public IActionResult Dashboard()
        {
            var stats = _service.GetStatistics();
            return View(stats);
        }
        
        [HttpGet]
        public IActionResult GetStats()
        {
            var stats = _service.GetStatistics();
            return Json(stats);
        }
    }
}
