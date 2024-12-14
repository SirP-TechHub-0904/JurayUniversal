using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class Attendance
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Profile User { get; set; }
        [Display(Name = "Start Time")]
        public DateTime? StartTime { get; set; }
        [Display(Name = "Close Time")]
        public DateTime? CloseTime { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        [Display(Name = "Status")]
        public AttendanceStatus AttendanceStatus { get; set; }
        public PeriodStatus ArivalPeriodStatus { get; set; }
        public PeriodStatus ClosePeriodStatus { get; set; }
    }
}
