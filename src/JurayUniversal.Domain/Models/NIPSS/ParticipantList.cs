using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class ParticipantList
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public Profile User { get; set; }
        public long? CourseId { get; set; }
        public Course Course { get; set; }
        public string IdNumber { get; set; }
        public int SerialNumber { get; set; } 
        //public long? AccomodationId { get; set; }
        public Accomodation Accomodation { get; set; }
        public long? StudyGroupId { get; set; }
        public StudyGroup StudyGroup { get; set; }
        public ParticipantStatus ParticipantStatus { get; set; }
         
    }
}
