using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class StaffManager
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public Profile User {  get; set; }
        public string? StaffId { get; set; }
        
        public long? StudyGroupId { get; set; }
        public StudyGroup StudyGroup { get; set; }

    }
}
