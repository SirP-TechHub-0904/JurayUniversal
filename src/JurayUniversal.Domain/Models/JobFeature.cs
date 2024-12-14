using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class JobFeature
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? JobId { get; set; }
        public Job Job { get; set; }
        public bool Paid { get; set; }
        public bool Compulsory { get; set; }
        public bool Selected { get; set; }
        public bool ForPrivate { get; set; }
        public int SortOrder { get; set; }
        public decimal Price { get; set; }
        public bool Disable { get; set; }
        public int DaysToComplete { get; set; }
    }
}
