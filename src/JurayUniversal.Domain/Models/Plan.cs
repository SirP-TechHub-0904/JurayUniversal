using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class Plan
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool ForPrivate { get; set; }
        public bool Disable { get; set; }
        public bool Compulsory { get; set; }
        public int DaysToComplete { get; set; }
        public int SortOrder { get; set; }
        public long JobCategoryId { get; set; }
        public JobCategory JobCategory { get; set; }
    }
}
