using Microsoft.AspNetCore.Mvc;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly IWebHostEnvironment _env;

        // 🔹 Static in-memory list (replaces the database)
        private static List<ReportIssue> _issues = new List<ReportIssue>();

        public ReportIssuesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ReportIssue issue, IFormFile Attachment)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (Attachment != null && Attachment.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);

                    var filePath = Path.Combine(uploads, Attachment.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Attachment.CopyTo(stream);
                    }
                    issue.AttachmentPath = "/uploads/" + Attachment.FileName;
                }

                // 🔹 Add to the in-memory list instead of DB
                issue.DateReported = DateTime.Now; // set timestamp
                _issues.Add(issue);

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

            return View(issue);
        }

        public IActionResult Index()
        {
            // 🔹 Return all issues from the in-memory list
            return View(_issues);
        }
    }
}
