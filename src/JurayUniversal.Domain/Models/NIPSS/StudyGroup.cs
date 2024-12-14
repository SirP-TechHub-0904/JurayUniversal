using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class StudyGroup
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public Profile User { get; set; }
        public string? Position { get; set; }
        

        public long StudyGroupCategoryId { get; set; }
        public StudyGroupCategory StudyGroupCategory { get; set; }
    }
}
