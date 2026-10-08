using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JurayUniversal.Domain.Models
{
    /// <summary>
    /// MisCommunityForm: public clan / community profiling application submitted via the
    /// shareable "/CommunityForm" URL. Managed under the MisUserManagement module.
    /// Multi-value fields (skills, languages, etc.) are stored as comma-separated strings.
    /// </summary>
    [Table("MisCommunityFormSubmissions")]
    public class MisCommunityFormSubmission
    {
        [Key]
        public long Id { get; set; }

        /// <summary>Public reference number given to the applicant (e.g. CF-20261008-AB12CD).</summary>
        [MaxLength(40)]
        public string? ReferenceNumber { get; set; }

        // ── 1. Basic Information ──────────────────────────────
        [Required, MaxLength(200), Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [MaxLength(100), Display(Name = "Preferred Name")]
        public string? PreferredName { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        [Display(Name = "Date of Birth"), DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(30), Display(Name = "Marital Status")]
        public string? MaritalStatus { get; set; }

        [Required, MaxLength(30), Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(200), EmailAddress, Display(Name = "Email Address")]
        public string? Email { get; set; }

        [MaxLength(500), Display(Name = "Current Residential Address")]
        public string? ResidentialAddress { get; set; }

        [MaxLength(150), Display(Name = "State/Country of Residence")]
        public string? StateCountryOfResidence { get; set; }

        // ── 2. Family Information ─────────────────────────────
        [MaxLength(200), Display(Name = "Father's Full Name")]
        public string? FatherFullName { get; set; }

        [MaxLength(200), Display(Name = "Mother's Full Name")]
        public string? MotherFullName { get; set; }

        [MaxLength(200), Display(Name = "Name of Family/Kindred")]
        public string? FamilyKindredName { get; set; }

        [MaxLength(200), Display(Name = "Name of Clan")]
        public string? ClanName { get; set; }

        [MaxLength(200), Display(Name = "Spouse's Name")]
        public string? SpouseName { get; set; }

        [Display(Name = "Number of Children")]
        public int? NumberOfChildren { get; set; }

        [MaxLength(1000), Display(Name = "Names of Children")]
        public string? ChildrenNames { get; set; }

        [MaxLength(200), Display(Name = "Relationship to the Clan/Community")]
        public string? RelationshipToCommunity { get; set; }

        // ── 3. Ancestral Information ──────────────────────────
        [MaxLength(200), Display(Name = "Father's Village/Town")]
        public string? FatherVillageTown { get; set; }

        [MaxLength(200), Display(Name = "Mother's Village/Town")]
        public string? MotherVillageTown { get; set; }

        [MaxLength(200), Display(Name = "Family Compound")]
        public string? FamilyCompound { get; set; }

        [MaxLength(200), Display(Name = "Kindred/Lineage")]
        public string? KindredLineage { get; set; }

        [MaxLength(200), Display(Name = "Name of Family Head")]
        public string? FamilyHeadName { get; set; }

        [MaxLength(300), Display(Name = "Ancestral Home")]
        public string? AncestralHome { get; set; }

        [MaxLength(4000), Display(Name = "Known Family History")]
        public string? FamilyHistory { get; set; }

        [MaxLength(1000), Display(Name = "Other Related Families/Kindreds")]
        public string? RelatedFamilies { get; set; }

        // Family tree
        [MaxLength(200), Display(Name = "Paternal Grandfather")]
        public string? PaternalGrandfather { get; set; }

        [MaxLength(200), Display(Name = "Paternal Grandmother")]
        public string? PaternalGrandmother { get; set; }

        [MaxLength(200), Display(Name = "Maternal Grandfather")]
        public string? MaternalGrandfather { get; set; }

        [MaxLength(200), Display(Name = "Maternal Grandmother")]
        public string? MaternalGrandmother { get; set; }

        [MaxLength(1000), Display(Name = "Great-Grandparents")]
        public string? GreatGrandparents { get; set; }

        // ── 4. Education ──────────────────────────────────────
        [MaxLength(100), Display(Name = "Highest Educational Qualification")]
        public string? HighestQualification { get; set; }

        [MaxLength(200), Display(Name = "School/Institution")]
        public string? Institution { get; set; }

        [MaxLength(200), Display(Name = "Course/Field of Study")]
        public string? FieldOfStudy { get; set; }

        [MaxLength(1000), Display(Name = "Professional Certifications")]
        public string? ProfessionalCertifications { get; set; }

        [MaxLength(200), Display(Name = "Current Course of Study")]
        public string? CurrentCourseOfStudy { get; set; }

        // ── 5. Employment & Business ──────────────────────────
        [MaxLength(150), Display(Name = "Current Occupation")]
        public string? CurrentOccupation { get; set; }

        [MaxLength(200), Display(Name = "Employer/Organisation")]
        public string? Employer { get; set; }

        [MaxLength(150), Display(Name = "Position/Job Title")]
        public string? JobTitle { get; set; }

        [MaxLength(200), Display(Name = "Business Name")]
        public string? BusinessName { get; set; }

        [MaxLength(150), Display(Name = "Business Type")]
        public string? BusinessType { get; set; }

        [MaxLength(300), Display(Name = "Business Location")]
        public string? BusinessLocation { get; set; }

        [Display(Name = "Years of Experience")]
        public int? YearsOfExperience { get; set; }

        [MaxLength(1000), Display(Name = "Professional Skills")]
        public string? ProfessionalSkills { get; set; }

        // ── 6. Skills & Expertise ─────────────────────────────
        [MaxLength(1000), Display(Name = "Areas of Expertise")]
        public string? ExpertiseAreas { get; set; }

        [MaxLength(300), Display(Name = "Other Expertise")]
        public string? OtherExpertise { get; set; }

        /// <summary>Yes / No / Maybe</summary>
        [MaxLength(10), Display(Name = "Willing to Offer Skills")]
        public string? WillingToOfferSkills { get; set; }

        // ── 7. Community Participation ────────────────────────
        [MaxLength(200), Display(Name = "Current Community/Clan Position")]
        public string? CurrentCommunityPosition { get; set; }

        [MaxLength(1000), Display(Name = "Previous Leadership Positions")]
        public string? PreviousLeadershipPositions { get; set; }

        [MaxLength(1000), Display(Name = "Membership of Community Associations")]
        public string? CommunityAssociations { get; set; }

        [MaxLength(2000), Display(Name = "Contributions to Community Development")]
        public string? CommunityContributions { get; set; }

        [MaxLength(1000), Display(Name = "Areas You Would Like to Contribute")]
        public string? ContributionInterests { get; set; }

        [MaxLength(1000), Display(Name = "Volunteer Interests")]
        public string? VolunteerInterests { get; set; }

        // ── 8. Contact & Emergency ────────────────────────────
        [MaxLength(30), Display(Name = "Alternative Phone Number")]
        public string? AlternativePhone { get; set; }

        [MaxLength(200), Display(Name = "Emergency Contact Name")]
        public string? EmergencyContactName { get; set; }

        [MaxLength(30), Display(Name = "Emergency Contact Phone")]
        public string? EmergencyContactPhone { get; set; }

        [MaxLength(100), Display(Name = "Relationship to Emergency Contact")]
        public string? EmergencyContactRelationship { get; set; }

        // ── 9. Optional Personal Information ──────────────────
        [MaxLength(500), Display(Name = "Passport Photograph")]
        public string? PassportPhotoUrl { get; set; }

        [MaxLength(300), Display(Name = "Social Media/Profile Link")]
        public string? SocialProfileLink { get; set; }

        [MaxLength(1000), Display(Name = "Hobbies/Interests")]
        public string? Hobbies { get; set; }

        [MaxLength(500), Display(Name = "Languages Spoken")]
        public string? LanguagesSpoken { get; set; }

        // ── 10. Consent ───────────────────────────────────────
        [Display(Name = "Declaration and Consent")]
        public bool ConsentGiven { get; set; }

        public DateTime? ConsentDateUtc { get; set; }

        // ── Admin & Draft Workflow ───────────────────────────
        /// <summary>Indicates if this record is a work-in-progress draft saved to continue later.</summary>
        public bool IsDraft { get; set; } = false;

        /// <summary>Optional 4-6 digit PIN or password to securely resume the draft from any device.</summary>
        [MaxLength(255)]
        public string? AccessPinHash { get; set; }

        /// <summary>The last step/tab number (1-5) the user was on when they saved.</summary>
        public int LastActiveStep { get; set; } = 1;

        /// <summary>Draft last auto-saved timestamp.</summary>
        public DateTime? LastSavedAtUtc { get; set; }

        /// <summary>Pending, Approved, Rejected</summary>
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [MaxLength(2000), Display(Name = "Admin Notes")]
        public string? AdminNotes { get; set; }

        [MaxLength(450)]
        public string? ReviewedByUserId { get; set; }

        public DateTime? ReviewedAtUtc { get; set; }

        [MaxLength(64)]
        public string? SubmittedFromIp { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
