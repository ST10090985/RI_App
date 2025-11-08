using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly ReportIssueQueue _reportQueue;

        // Inject the queue (in-memory)
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
        public IActionResult Create(ReportIssue issue)
        {
            if (ModelState.IsValid)
            {
                _reportQueue.AddIssue(issue); // ✅ updated method name

                TempData["SuccessMessage"] = "Issue reported successfully!";
                return RedirectToAction("Create");
            }

            return View(issue);
        }

        // GET: /ReportIssues/ListIssues
        [HttpGet]
        public IActionResult ListIssues()
        {
            var issues = _reportQueue.GetAllIssues(); // ✅ updated method name
            return View(issues);
        }

        // POST: /ReportIssues/UpdateStatus
        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            _reportQueue.UpdateStatus(id, newStatus); // ✅ method unchanged
            TempData["SuccessMessage"] = "Issue status updated successfully!";
            return RedirectToAction("ListIssues");
        }

        // POST: /ReportIssues/RemoveNext
        [HttpPost]
        public IActionResult RemoveNext()
        {
            var removed = _reportQueue.GetNextPriorityIssue(); // ✅ uses MinHeap
            if (removed == null)
            {
                TempData["ErrorMessage"] = "No issues available to remove.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Removed issue from {removed.Location}.";
            }

            return RedirectToAction("ListIssues");
        }

        // (Optional) – show sorted issues by date (BST)
        [HttpGet]
        public IActionResult SortedIssues()
        {
            var sorted = _reportQueue.GetIssuesSortedByDate();
            return View("ListIssues", sorted);
        }

        // (Optional) – recommend issues by category (Graph)
        [HttpGet]
        public IActionResult Recommend(string category)
        {
            var related = _reportQueue.RecommendRelated(category);
            return View("ListIssues", related);
        }
    }
}
