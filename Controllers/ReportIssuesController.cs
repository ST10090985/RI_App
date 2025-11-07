using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly ReportIssueQueue _reportQueue;

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
                _reportQueue.Add(issue);

                TempData["SuccessMessage"] = "Issue reported successfully!";
                return RedirectToAction("Create");
            }

            // If validation fails, redisplay form
            return View(issue);
        }

        // GET: /ReportIssues/ListIssues
        public IActionResult ListIssues()
        {
            var issues = _reportQueue.GetAll();
            return View(issues);
        }
    }
}
