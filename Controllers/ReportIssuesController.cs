using Microsoft.AspNetCore.Mvc;
using RI_App.Data;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        // Constructor: injects the database context and web hosting environment
        public ReportIssuesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
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
        public IActionResult Create(ReportIssue issue, IFormFile Attachment)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload (if user attached a file)
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

                // Save issue to the database
                _context.ReportIssues.Add(issue);
                _context.SaveChanges();

                // Show a confirmation message when redirected back to form
                TempData["SuccessMessage"] = "Issue reported successfully!";
                return RedirectToAction("Create");
            }
            else
            {
                // Logs validation errors to console (useful for debugging)
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine(string.Join(",", errors));
            }

            // If model validation failed, redisplay form with entered values
            return View(issue);
        }

        // GET: /ReportIssues/Index
        // Displays a list of all reported issues in a table
        public IActionResult Index()
        {
            var issues = _context.ReportIssues.ToList();
            return View(issues);
        }
    }
}
