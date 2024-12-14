using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class XProject
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Full Description")]
        public string FullDescription { get; set; }
        [Display(Name = "Created Date")]

        public DateTime CreatedDate { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Date To Start Testing")]
        public DateTime DateToStartTesting { get; set; }
        [Display(Name = "Date To GoLive")]
        public DateTime DateToGoLive { get; set; }
        public Priority Priority { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public ICollection<XProjectSection> Sections { get; set; }
        public ICollection<XProjectMessage> Messages { get; set; }
    }
}
