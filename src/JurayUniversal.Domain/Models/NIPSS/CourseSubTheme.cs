using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class CourseSubTheme
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CourseId { get; set; }
        public Course Course { get; set; }
    }
}
