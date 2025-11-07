using RI_App.Models;

namespace RI_App.DataStructure
{
    /// <summary>
    /// The ReportIssueQueue class acts as an in-memory data structure
    /// that stores and manages all reported issues using a Queue-like behavior.
    /// It supports adding, retrieving, updating, and sorting issues,
    /// as well as optional file uploads.
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
                Directory.CreateDirectory(_uploadPath);
        }

        /// <summary>
        /// Adds a new issue to the list (acts like an Enqueue operation).
        /// Handles unique ID generation, status initialization, 
        /// and file attachment storage if available.
        /// </summary>
        public void Add(ReportIssue issue)
        {
            issue.Id = _nextId++;
            issue.Status = "Pending";
            issue.DateReported = DateTime.UtcNow;

            // Handle attachment upload (optional)
            if (issue.Attachment != null && issue.Attachment.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(issue.Attachment.FileName)}";
                string filePath = Path.Combine(_uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    issue.Attachment.CopyTo(stream);

                // Store the relative path for display
                issue.AttachmentPath = $"/uploads/{uniqueFileName}";
            }

            // Add to in-memory list
            _issues.Add(issue);

            // Keep list ordered by date (oldest first)
            _issues.Sort((a, b) => a.DateReported.CompareTo(b.DateReported));
        }

        /// <summary>
        /// Returns all stored issues as a list.
        /// </summary>
        public List<ReportIssue> GetAll() => _issues.ToList();

        /// <summary>
        /// Returns the first issue in the queue (Peek operation).
        /// </summary>
        public ReportIssue? Peek() => _issues.FirstOrDefault();

        /// <summary>
        /// Removes the oldest issue from the queue (Dequeue operation).
        /// </summary>
        public ReportIssue? RemoveNext()
        {
            if (_issues.Count == 0) return null;

            var next = _issues[0];
            _issues.RemoveAt(0);
            return next;
        }

        /// <summary>
        /// Updates the status of an existing issue.
        /// Returns true if the update was successful.
        /// </summary>
        public bool UpdateStatus(int id, string newStatus)
        {
            var issue = _issues.FirstOrDefault(i => i.Id == id);
            if (issue == null) return false;

            issue.Status = newStatus;
            return true;
        }

        /// <summary>
        /// Returns all issues with a specific status (e.g., "Pending").
        /// </summary>
        public List<ReportIssue> GetByStatus(string status)
        {
            return _issues
                .Where(i => i.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Returns all issues in a specific category (e.g., "Water", "Electricity").
        /// </summary>
        public List<ReportIssue> GetByCategory(string category)
        {
            return _issues
                .Where(i => i.Category != null &&
                            i.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Returns all issues reported after a certain date.
        /// </summary>
        public List<ReportIssue> GetByDate(DateTime fromDate)
        {
            return _issues
                .Where(i => i.DateReported >= fromDate)
                .OrderBy(i => i.DateReported)
                .ToList();
        }

        /// <summary>
        /// Returns the highest priority issue (based on category or date).
        /// This can simulate a "Priority Queue" structure.
        /// </summary>
        public ReportIssue? GetHighestPriorityIssue()
        {
            // Example: prioritize based on category keyword (e.g., "Emergency" or "Power")
            return _issues
                .OrderByDescending(i => i.Category?.Contains("Emergency") ?? false)
                .ThenBy(i => i.DateReported)
                .FirstOrDefault();
        }
    }
}
