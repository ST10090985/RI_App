using RI_App.Models;

namespace RI_App.DataStructure
{
    /// <summary>
    /// The ReportIssueQueue class acts as an in-memory data structure
    /// that stores and manages all reported issues (similar to a queue).
    /// It handles file uploads, issue sorting, and status updates.
    /// </summary>
    public class ReportIssueQueue
    {
        private readonly string _uploadPath;
        private readonly List<ReportIssue> _issues = new();
        private int _nextId = 1;

        public ReportIssueQueue(IWebHostEnvironment env)
        {
            // Define the folder where attachments will be saved
            _uploadPath = Path.Combine(env.WebRootPath, "uploads");

            // Ensure that the uploads folder exists
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        /// <summary>
        /// Adds a new issue to the list (acts like an enqueue operation).
        /// Assigns a unique ID, sets status to Pending, and handles file upload if available.
        /// </summary>
        public void Add(ReportIssue issue)
        {
            issue.Id = _nextId++;
            issue.Status = "Pending"; // Default status

            // Handle attachment upload if provided
            if (issue.Attachment != null && issue.Attachment.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(issue.Attachment.FileName)}";
                string filePath = Path.Combine(_uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    issue.Attachment.CopyTo(stream);
                }

                // Store relative web path for display in the UI
                issue.AttachmentPath = $"/uploads/{uniqueFileName}";
            }

            // Add to in-memory list
            _issues.Add(issue);

            // Keep issues ordered by date (oldest first)
            _issues.Sort((a, b) => a.DateReported.CompareTo(b.DateReported));
        }

        /// <summary>
        /// Returns all currently stored issues.
        /// </summary>
        public List<ReportIssue> GetAll() => _issues.ToList();

        /// <summary>
        /// Returns the first issue in the queue without removing it (peek).
        /// </summary>
        public ReportIssue? Peek() => _issues.FirstOrDefault();

        /// <summary>
        /// Removes the oldest issue from the queue (dequeue).
        /// </summary>
        public ReportIssue? RemoveNext()
        {
            if (_issues.Count == 0) return null;

            var next = _issues[0];
            _issues.RemoveAt(0);
            return next;
        }

        /// <summary>
        /// Updates the status of a specific issue by its ID.
        /// </summary>
        public bool UpdateStatus(int id, string newStatus)
        {
            var issue = _issues.FirstOrDefault(i => i.Id == id);
            if (issue == null) return false;

            issue.Status = newStatus;
            return true;
        }

        /// <summary>
        /// Returns all issues matching a specific status (optional helper method).
        /// </summary>
        public List<ReportIssue> GetByStatus(string status)
        {
            return _issues
                .Where(i => i.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
