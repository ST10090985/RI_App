using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;

namespace RI_App.Controllers
{
    /// <summary>
    /// Handles all issue reporting features using an in-memory data structure (ReportIssueQueue).
    /// This controller does not save data permanently; all reports are cleared when the app stops running.
    /// </summary>
    public class ReportIssuesController : Controller
    {
        private readonly ReportIssueQueue _reportQueue;

        // Inject our in-memory queue (registered in Program.cs)
        public ReportIssuesController(ReportIssueQueue reportQueue)
        {
            _reportQueue = reportQueue;
        }

        // GET: /ReportIssues/Create
        // Displays the "Report Issue" form to the user.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ReportIssues/Create
        // Handles form submission and adds a new issue to the in-memory queue.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReportIssue issue)
        {
            if (ModelState.IsValid)
            {
                // Add the issue to the in-memory list
                _reportQueue.Add(issue);

                // Show confirmation message
                TempData["SuccessMessage"] = "Issue reported successfully!";
                return RedirectToAction(nameof(Create));
            }

            // If validation fails, show form again with errors
            return View(issue);
        }

        // GET: /ReportIssues/ListIssues
        // Displays all issues currently stored in the queue
        [HttpGet]
        public IActionResult ListIssues()
        {
            var issues = _reportQueue.GetAll();

            // If no issues yet, view can handle showing "no data" message
            return View(issues);
        }

        // POST: /ReportIssues/UpdateStatus
        // Updates the status of a selected issue (e.g., Pending → Resolved)
        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            bool updated = _reportQueue.UpdateStatus(id, newStatus);

            if (updated)
                TempData["SuccessMessage"] = "Issue status updated successfully!";
            else
                TempData["ErrorMessage"] = "Could not find issue to update.";

            return RedirectToAction("ListIssues");
        }

        // POST: /ReportIssues/RemoveNext
        // Removes the oldest issue from the queue (simulates processing or completion)
        [HttpPost]
        public IActionResult RemoveNext()
        {
            var removed = _reportQueue.RemoveNext();

            if (removed == null)
                TempData["ErrorMessage"] = "No issues available to remove.";
            else
                TempData["SuccessMessage"] = $"Removed issue from {removed.Location}.";

            return RedirectToAction("ListIssues");
        }
    }
}
