using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisGroupFile
    {
        public long Id { get; set; }

        public long MisGroupId { get; set; }
        public MisGroup MisGroup { get; set; }

        public string UploadedByUserId { get; set; } = string.Empty;
        public Profile UploadedByUser { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "File Name")]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "File URL")]
        public string FileUrl { get; set; } = string.Empty;

        [Display(Name = "Date Uploaded")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
