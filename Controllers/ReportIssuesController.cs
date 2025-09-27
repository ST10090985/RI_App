using Microsoft.AspNetCore.Mvc;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly IWebHostEnvironment _env;

        // Static in-memory list (replaces the database)
        private static List<ReportIssue> _issues = new List<ReportIssue>();

        public ReportIssuesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET: /ReportIssues/Create
        // Shows the form where a user can report a new issue
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ReportIssues/Create
        // Handles form submission for new issue reports
        [HttpPost]
        public IActionResult Create(ReportIssue issue, IFormFile? Attachment)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (Attachment != null && Attachment.Length > 0)
                {
                    // Save uploaded files under wwwroot/uploads
                    var uploads = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    var filePath = Path.Combine(uploads, Attachment.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Attachment.CopyTo(stream);
                    }

                    // Store relative path in database
                    issue.AttachmentPath = "/uploads/" + Attachment.FileName;
                }

                // 🔹 Add to the in-memory list instead of DB
                issue.DateReported = DateTime.Now; // set timestamp
                _issues.Add(issue);

                // Show a confirmation message when redirected back to form
                TempData["SuccessMessage"] = "Issue reported successfully!";
                return RedirectToAction("Create");
            }
            else
            {
                // Debugging validation errors
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                Console.WriteLine(string.Join(",", errors));
            }

            // If model validation failed, redisplay form with entered values
            return View(issue);
        }

        // GET: /ReportIssues/Index
        // Displays a list of all reported issues in a table
        public IActionResult Index()
        {
            // 🔹 Return all issues from the in-memory list
            return View(_issues);
        }
    }
}
