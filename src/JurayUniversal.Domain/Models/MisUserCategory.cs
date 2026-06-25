using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// MIS: Defines the category/type of a user within the organization.
    /// e.g., "Staff", "Volunteer", "Alumni", "Member", "Contractor"
    /// Every tenant creates their own categories specific to their organization.
    /// </summary>
    public class MisUserCategory
    {
        public long Id { get; set; }

        [Display(Name = "Category Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
