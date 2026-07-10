using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisEquipmentCategory
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<MisEquipment> MisEquipments { get; set; }
    }
}
