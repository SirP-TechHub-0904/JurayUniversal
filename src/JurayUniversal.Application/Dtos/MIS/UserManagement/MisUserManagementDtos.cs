using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Application.Dtos.MIS.UserManagement
{
    /// <summary>
    /// DTO for creating or editing a KPI Indicator (the admin-defined performance criteria).
    /// </summary>
    public class MisKpiIndicatorDto
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Indicator Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Maximum Score")]
        public decimal MaxScore { get; set; } = 100;

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO for submitting a single KPI score for one user on one indicator.
    /// A list of these is submitted at once from the KpiAssign page.
    /// </summary>
    public class MisKpiScoreDto
    {
        public long Id { get; set; }

        [Required]
        public string ProfileId { get; set; }

        [Required]
        public long MisKpiIndicatorId { get; set; }

        public string? IndicatorTitle { get; set; }   // display only
        public decimal MaxScore { get; set; }          // display only

        [Required]
        [Display(Name = "Score")]
        public decimal Score { get; set; }

        [Required]
        [Display(Name = "Evaluation Period")]
        public string Period { get; set; }             // e.g., "Q1 2025"

        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        public string? EvaluatedBy { get; set; }       // set server-side
    }

    /// <summary>
    /// DTO for a single row in the KPI Leaderboard / rankings table.
    /// </summary>
    public class MisKpiLeaderboardDto
    {
        public string ProfileId { get; set; }
        public string FullName { get; set; }
        public string? PassportUrl { get; set; }
        public decimal TotalScore { get; set; }
        public decimal MaxPossibleScore { get; set; }
        public decimal Percentage { get; set; }
        public int Rank { get; set; }
        public string? MisUserCategoryName { get; set; }
    }

    /// <summary>
    /// DTO for assigning or editing a Feature Role for a specific user.
    /// </summary>
    public class MisUserFeatureRoleDto
    {
        public long Id { get; set; }

        [Required]
        public string ProfileId { get; set; }

        [Required]
        [Display(Name = "Feature Area")]
        public string FeatureAreaKey { get; set; }      // e.g., "Elections", "Ticketing"

        [Required]
        [Display(Name = "Action Role")]
        public int ActionRole { get; set; }             // maps to MisActionRole enum

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public string? AssignedBy { get; set; }         // set server-side
    }

    /// <summary>
    /// DTO for adding a private admin note to a user's profile.
    /// </summary>
    public class MisUserNoteDto
    {
        public long Id { get; set; }

        [Required]
        public string ProfileId { get; set; }

        [Required]
        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Private (Admin Eyes Only)")]
        public bool IsPrivate { get; set; } = true;

        public string? CreatedBy { get; set; }          // set server-side
    }

    /// <summary>
    /// DTO for creating or editing a User Category (Staff, Volunteer, Alumni, etc.)
    /// </summary>
    public class MisUserCategoryDto
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
