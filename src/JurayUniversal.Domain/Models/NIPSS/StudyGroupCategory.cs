using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class StudyGroupCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Slogan { get; set; }

        public string? GroupRules { get; set; }

        public ICollection<StudyGroup> StudyGroups { get; set; }

        public long CourseId { get; set; }
        public Course Course { get; set;}
    }
}
