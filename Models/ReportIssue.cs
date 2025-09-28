using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace RI_App.Models
{
    public class ReportIssue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string? Category { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        // For uploaded file (from form)
        public IFormFile? Attachment { get; set; }
        public string? AttachmentPath { get; set; } = string.Empty;

        public DateTime DateReported { get; set; } = DateTime.UtcNow;
    }
}
