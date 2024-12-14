using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class XProjectFile
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long XProjectId { get; set; }
        public XProject XProject { get; set; }

        public string? Url { get; set; }
        public string? Key { get; set; }
    }
}
