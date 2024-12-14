using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class TimeTable
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public int SortOrder { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Content { get; set; }
        public ICollection<Moderator> Moderators { get; set; } 
        public EventType EventType { get; set; }
        public ContentStatus ContentStatus { get; set; }
        public ContentEnumType ContentType { get; set; }
        public string? Note { get; set; }

        public bool IsLecture { get; set; }
        public string? Module { get; set; }

        public long? CourseId { get; set; }
        public Course Course { get; set; }

    }
}
