using Microsoft.AspNetCore.Mvc;
using VisitorLogAPI.Models;
using VisitorLogAPI.Services;

namespace VisitorLogAPI.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly VisitorLogService _service;
        private readonly IConfiguration _configuration;

        public VisitorsController(VisitorLogService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString("VisitorAuth") == "true";
        }

        private IActionResult RequireAuth()
        {
            return RedirectToAction("Authenticate");
        }

        public IActionResult Authenticate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string password)
        {
            var expectedPassword = _configuration["VisitorPassword"] ?? "visitor123";
            if (password == expectedPassword)
            {
                HttpContext.Session.SetString("VisitorAuth", "true");
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid password.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("VisitorAuth");
            return RedirectToAction("Authenticate");
        }

        public IActionResult Index(string? search, string? office, string? municipality, string? barangay, DateTime? dateFrom, DateTime? dateTo)
        {
            if (!IsAuthenticated()) return RequireAuth();
            var logs = _service.GetAll(search, office, municipality, barangay, dateFrom, dateTo);
            ViewBag.Search = search;
            ViewBag.Office = office;
            ViewBag.Municipality = municipality;
            ViewBag.Barangay = barangay;
            ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
            ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");
            return View(logs);
        }

        public IActionResult Create()
        {
            if (!IsAuthenticated()) return RequireAuth();
            return View();
        }

        [HttpPost]
        public IActionResult Create(VisitorLog log)
        {
            if (!IsAuthenticated()) return RequireAuth();
            if (ModelState.IsValid)
            {
                _service.Create(log);
                return RedirectToAction("Index");
            }
            return View(log);
        }

        public IActionResult Edit(int id)
        {
            if (!IsAuthenticated()) return RequireAuth();
            var log = _service.GetById(id);
            if (log == null) return NotFound();
            return View(log);
        }

        [HttpPost]
        public IActionResult Edit(VisitorLog log)
        {
            if (!IsAuthenticated()) return RequireAuth();
            if (ModelState.IsValid)
            {
                _service.Update(log);
                return RedirectToAction("Index");
            }
            return View(log);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAuthenticated()) return RequireAuth();
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
