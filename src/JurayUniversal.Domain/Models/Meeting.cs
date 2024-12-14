using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class Meeting
    {
        public long Id { get; set; }
        public string? Minute { get; set; }
        [Display(Name = "Start Time")]
        public DateTime? StartTime { get; set; }
        [Display(Name = "Close Time")]
        public DateTime? CloseTime { get; set; }
        [Display(Name = "Meeting Attendances")]
        public ICollection<MeetingAttendance> MeetingAttendances { get; set; }

        public string? Comment { get; set; }
        public bool CloseUpdate { get; set; }
    }
}
