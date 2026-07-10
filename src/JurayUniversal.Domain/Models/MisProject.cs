using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisProject
    {
        public long Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Enum.Enum.MisProjectStatus Status { get; set; } = Enum.Enum.MisProjectStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<MisProjectSprint> Sprints { get; set; } = new List<MisProjectSprint>();
        public ICollection<MisProjectTask> Tasks { get; set; } = new List<MisProjectTask>();
        public ICollection<MisProjectUser> ProjectUsers { get; set; } = new List<MisProjectUser>();
    }
}
