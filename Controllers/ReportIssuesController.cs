using Microsoft.AspNetCore.Mvc;
using RI_App.Data;
using RI_App.Models;

namespace RI_App.Controllers
{
    public class ReportIssuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReportIssuesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
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

                _context.ReportIssues.Add(issue);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Issue reported successfully!";
                return RedirectToAction("Create");
            }
            return View(issue);
        }

        public IActionResult Index()
        {
            var issues = _context.ReportIssues.ToList();
            return View(issues);
        }
    }
}
