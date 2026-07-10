using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisProjectSprint
    {
        public long Id { get; set; }

        public long MisProjectId { get; set; }
        public MisProject? MisProject { get; set; }

        [Required, StringLength(100)]
        public string SprintName { get; set; }

        public string? Goal { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Enum.Enum.MisSprintStatus Status { get; set; } = Enum.Enum.MisSprintStatus.Planned;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<MisProjectTask> Tasks { get; set; } = new List<MisProjectTask>();
    }
}
