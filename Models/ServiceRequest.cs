using System;

namespace RI_App.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }                  // Unique ID
        public string? Title { get; set; }            // Short description
        public string? Description { get; set; }      // Details
        public string? Status { get; set; }           // Pending, In Progress, Completed
        public int? Progress { get; set; }            // 0–100 (optional, for progress bar)
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
