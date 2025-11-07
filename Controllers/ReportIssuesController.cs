using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly ReportIssueQueue _reportQueue;

        // Inject the queue (acts as our in-memory "database")
        public ReportIssuesController(ReportIssueQueue reportQueue)
        {
            _reportQueue = reportQueue;
        }

        // GET: /ReportIssues/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ReportIssues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReportIssue issue)
        {
            if (ModelState.IsValid)
            {
                // Add issue to queue (stored in memory, not database)
                _reportQueue.Add(issue);

                TempData["SuccessMessage"] = "Issue reported successfully!";
                return RedirectToAction(nameof(Create));
            }

            // If validation fails, redisplay the same form with errors
            return View(issue);
        }

        // GET: /ReportIssues/ListIssues
        // Displays all reported issues from the in-memory queue
        [HttpGet]
        public IActionResult ListIssues()
        {
            var issues = _reportQueue.GetAll();
            return View(issues);
        }

        // POST: /ReportIssues/UpdateStatus
        // Allows changing issue status (e.g., from Pending to Resolved)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
            {
                TempData["ErrorMessage"] = "Invalid status value.";
                return RedirectToAction(nameof(ListIssues));
            }

            bool updated = _reportQueue.UpdateStatus(id, newStatus);

            TempData[updated ? "SuccessMessage" : "ErrorMessage"] =
                updated
                    ? $"Issue #{id} status updated to '{newStatus}'."
                    : "Issue not found or could not be updated.";

            return RedirectToAction(nameof(ListIssues));
        }

        // POST: /ReportIssues/RemoveNext
        // Simulates processing (removes the oldest issue from the queue)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveNext()
        {
            var removed = _reportQueue.RemoveNext();

            TempData[removed == null ? "ErrorMessage" : "SuccessMessage"] =
                removed == null
                    ? "No issues available to remove."
                    : $"Removed issue from {removed.Location} (ID: {removed.Id}).";

            return RedirectToAction(nameof(ListIssues));
        }

        // Optional: Filter issues by status
        [HttpGet]
        public IActionResult FilterByStatus(string status)
        {
            var filtered = string.IsNullOrEmpty(status)
                ? _reportQueue.GetAll()
                : _reportQueue.GetByStatus(status);

            ViewBag.SelectedStatus = status;
            return View("ListIssues", filtered);
        }
    }
}
