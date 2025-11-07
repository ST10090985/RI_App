using RI_App.Models;

namespace RI_App.DataStructure
{
    public class ReportIssueQueue
    {
        private readonly string _uploadPath;
        private readonly List<ReportIssue> _issues = new();
        private int _nextId = 1;

        public ReportIssueQueue(IWebHostEnvironment env)
        {
            // Ensure uploads folder exists
            _uploadPath = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        // Adds a new issue to the list (acts like enqueue)
        public void Add(ReportIssue issue)
        {
            issue.Id = _nextId++;
            issue.Status = "Pending";

            // Save attachment if available
            if (issue.Attachment != null && issue.Attachment.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(issue.Attachment.FileName)}";
                string filePath = Path.Combine(_uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    issue.Attachment.CopyTo(stream);
                }

                issue.AttachmentPath = $"/uploads/{uniqueFileName}";
            }

            _issues.Add(issue);

            // Keep list sorted by date (oldest first)
            _issues.Sort((a, b) => a.DateReported.CompareTo(b.DateReported));
        }

        // Returns all issues currently stored
        public List<ReportIssue> GetAll() => _issues.ToList();

        // Gets the next issue in line (peek)
        public ReportIssue? Peek() => _issues.FirstOrDefault();

        // Removes the first issue (dequeue)
        public ReportIssue? RemoveNext()
        {
            if (_issues.Count == 0) return null;
            var next = _issues[0];
            _issues.RemoveAt(0);
            return next;
        }

        // Updates an issue's status by Id
        public bool UpdateStatus(int id, string newStatus)
        {
            var issue = _issues.FirstOrDefault(i => i.Id == id);
            if (issue == null) return false;

            issue.Status = newStatus;
            return true;
        }

        // Returns only issues with a certain status (optional helper)
        public List<ReportIssue> GetByStatus(string status)
        {
            return _issues.Where(i => i.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
