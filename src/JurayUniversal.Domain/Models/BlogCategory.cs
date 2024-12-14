using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class BlogCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public ICollection<Blog> Blogs { get; set; }
        public bool Publish { get; set; }

    }
}
