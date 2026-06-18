using Microsoft.AspNetCore.Mvc;
using OSAWebAPI.Services;

namespace OSAWebAPI.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly VisitorLogService _service;

        public VisitorsController(VisitorLogService service)
        {
            _service = service;
        }

        public IActionResult Index(string? search, string? office, string? municipality, string? barangay, DateTime? dateFrom, DateTime? dateTo)
        {
            var logs = _service.GetAll(search, office, municipality, barangay, dateFrom, dateTo);
            ViewBag.Search = search;
            ViewBag.Office = office;
            ViewBag.Municipality = municipality;
            ViewBag.Barangay = barangay;
            ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");
            return View(logs);
        }
    }
}
