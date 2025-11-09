using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly ReportIssueQueue _reportQueue;

        // Inject our data structure (singleton registered in Program.cs)
        public ReportIssuesController(ReportIssueQueue reportQueue)
        {
            _reportQueue = reportQueue;
        }

        // ==============================
        // List All Reported Issues
        // ==============================
        [HttpGet]
        public IActionResult ListIssues()
        {
            var issues = _reportQueue.GetAllIssues();
            return View(issues);
        }

        // ==============================
        // Show Create Issue Page
        // ==============================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ==============================
        // Submit New Issue
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReportIssue issue, IFormFile? attachment)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid issue details. Please try again.";
                return View(issue);
            }

            // Handle attachment upload
            if (attachment != null && attachment.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/uploads", attachment.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    attachment.CopyTo(stream);
                }
                issue.AttachmentPath = "/uploads/" + attachment.FileName;
            }

            // Set defaults
            issue.DateReported = DateTime.Now;
            issue.Status = "Pending";

            // Add issue to all data structures
            _reportQueue.AddIssue(issue);

            TempData["SuccessMessage"] = "Issue reported successfully!";
            return RedirectToAction("ListIssues");
        }

        // ==============================
        // Remove Highest Priority Issue
        // ==============================
        [HttpPost]
        public IActionResult RemoveHighestPriority()
        {
            var removed = _reportQueue.RemoveHighestPriorityIssue();

            if (removed != null)
                TempData["SuccessMessage"] = $"Removed highest priority issue from {removed.Location}.";
            else
                TempData["ErrorMessage"] = "No issues available to remove.";

            return RedirectToAction("ListIssues");
        }

        // ==============================
        // Update Issue Priority
        // ==============================
        [HttpPost]
        public IActionResult UpdatePriority(int id, int newPriority)
        {
            bool updated = _reportQueue.UpdatePriority(id, newPriority);

            if (updated)
                TempData["SuccessMessage"] = "Issue priority updated successfully.";
            else
                TempData["ErrorMessage"] = "Could not update issue priority.";

            return RedirectToAction("ListIssues");
        }

        // ==============================
        // Update Issue Status (Pending / Resolved)
        // ==============================
        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var issue = _reportQueue.GetAllIssues().FirstOrDefault(i => i.Id == id);
            if (issue != null)
            {
                issue.Status = newStatus;
                TempData["SuccessMessage"] = $"Issue status updated to '{newStatus}'.";
            }
            else
            {
                TempData["ErrorMessage"] = "Issue not found.";
            }

            return RedirectToAction("ListIssues");
        }

        // ==============================
        // Show Issues Sorted by Date (BST)
        // ==============================
        [HttpGet]
        public IActionResult SortedIssues()
        {
            var sorted = _reportQueue.GetIssuesSortedByDate();
            return View("ListIssues", sorted);
        }

        // ==============================
        // Recommend Related Issues (Graph)
        // ==============================
        [HttpGet]
        public IActionResult Recommend(string category)
        {
            var related = _reportQueue.RecommendedRelated(category);
            TempData["SuccessMessage"] = $"Showing related issues for category: {category}";
            return View("ListIssues", related);
        }
    }
}
