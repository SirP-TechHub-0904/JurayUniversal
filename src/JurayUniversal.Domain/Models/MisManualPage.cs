using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisManualPage
    {
        public MisManualPage()
        {
            MisManualElements = new HashSet<MisManualElement>();
        }

        [Key]
        public long Id { get; set; }

        public long MisManualId { get; set; }
        public MisManual MisManual { get; set; }

        [Required]
        public string RouteKey { get; set; }

        [Required]
        public string PageTitle { get; set; }

        public string PageDescription { get; set; }

        public ICollection<MisManualElement> MisManualElements { get; set; }
    }
}
