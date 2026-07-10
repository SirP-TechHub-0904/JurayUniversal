using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisProjectTask
    {
        public long Id { get; set; }

        public long MisProjectId { get; set; }
        public MisProject? MisProject { get; set; }

        public long? MisProjectSprintId { get; set; }
        public MisProjectSprint? MisProjectSprint { get; set; }

        public string? AssignedToUserId { get; set; }
        public Profile? AssignedToUser { get; set; }

        [Required, StringLength(255)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public Enum.Enum.MisTaskType Type { get; set; } = Enum.Enum.MisTaskType.Task;
        
        public Enum.Enum.MisTaskStatus Status { get; set; } = Enum.Enum.MisTaskStatus.ToDo;

        public Enum.Enum.MisTaskPriority Priority { get; set; } = Enum.Enum.MisTaskPriority.Medium;

        public int StoryPoints { get; set; } = 0;

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
