using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// MIS: Allows admins to attach private HR-style internal notes to any user's profile.
    /// This works like an activity log or case notes system.
    /// IsPrivate = true means only admins can see it.
    /// IsPrivate = false means the user can also see the note on their own profile.
    /// </summary>
    public class MisUserNote
    {
        public long Id { get; set; }

        // The user this note belongs to
        [Display(Name = "User")]
        [Required]
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }

        [Display(Name = "Note")]
        [Required]
        public string Note { get; set; }

        // Who wrote this note
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; } // ProfileId of the admin

        [Display(Name = "Private (Admin Eyes Only)")]
        public bool IsPrivate { get; set; } = true;

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
