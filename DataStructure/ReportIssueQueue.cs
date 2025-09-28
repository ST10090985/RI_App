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
            // Ensure files are saved in wwwroot/uploads
            _uploadPath = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public void Add(ReportIssue issue)
        {
            issue.Id = _nextId++;

            // Save attachment if uploaded
            if (issue.Attachment != null && issue.Attachment.Length > 0)
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(issue.Attachment.FileName)}";
                string filePath = Path.Combine(_uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    issue.Attachment.CopyTo(stream);
                }

                // Save relative path for later retrieval
                issue.AttachmentPath = $"/uploads/{uniqueFileName}";
            }

            _issues.Add(issue);

            // Sort by date reported
            _issues.Sort((a, b) => a.DateReported.CompareTo(b.DateReported));
        }

        public List<ReportIssue> GetEvents() => _issues.ToList();

        public ReportIssue? Peek() => _issues.Count > 0 ? _issues[0] : null;

        public ReportIssue? RemoveHighestPriority()
        {
            if (_issues.Count == 0) return null;
            var highest = _issues[0];
            _issues.RemoveAt(0);
            return highest;
        }
    }
}
