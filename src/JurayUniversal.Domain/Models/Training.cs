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
    public class Training
    {
        public long Id { get; set; }
        public string? Topic { get; set; }
        public string? DocumentUrl { get; set; }
        public string? DocumentKey { get; set; }
        [Display(Name = "Start Time")]
        public DateTime? StartTime { get; set; }

        public DateTime Date { get; set; }
        [Display(Name = "Close Time")]
        public DateTime? CloseTime { get; set; }
        [Display(Name = "Training Attendances")]
        public ICollection<TrainingAttendance> TrainingAttendances { get; set; }
        [Display(Name = "Questions And Answers")]
        public string? QuestionsAndAnswers { get; set; }
        public string? ModeratorId { get; set; }
        public Profile Moderator { get; set; }
        public string? TrainnerId { get; set; }
        public Profile Trainner { get; set; }
        public TrainingStatus TrainingStatus { get; set; }
    }
}
