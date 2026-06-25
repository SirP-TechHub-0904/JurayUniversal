using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisManual
    {
        public MisManual()
        {
            MisManualPages = new HashSet<MisManualPage>();
        }

        [Key]
        public long Id { get; set; }

        [Required]
        public string ModuleFolder { get; set; }

        [Required]
        public string Title { get; set; }

        public string Overview { get; set; }

        public ICollection<MisManualPage> MisManualPages { get; set; }
    }
}
