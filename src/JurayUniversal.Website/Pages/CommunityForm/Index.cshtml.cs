using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Pages.CommunityForm
{
    /// <summary>
    /// MisCommunityForm – public, shareable clan/community profiling application.
    /// Supports Tabbed Navigation, Realtime Auto-Saving, Password/PIN protection, and "Continue From Where You Stopped".
    /// Route: /CommunityForm
    /// </summary>
    [AllowAnonymous]
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedPhotoExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxPhotoBytes = 2 * 1024 * 1024; // 2 MB

        public IndexModel(DashboardDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public static readonly string[] ExpertiseOptions =
        {
            "Software/IT", "Medicine/Healthcare", "Law", "Engineering", "Education", "Agriculture",
            "Construction", "Finance", "Entrepreneurship", "Skilled Trades", "Administration", "Media/Communication"
        };

        public SuperSetting SuperSetting { get; set; } = new();
        public string OrgName { get; set; } = "Our Community";

        [BindProperty]
        public MisCommunityFormSubmission Input { get; set; } = new();

        [BindProperty]
        public List<string> SelectedExpertise { get; set; } = new();

        [BindProperty]
        public IFormFile? PassportPhoto { get; set; }

        [BindProperty]
        public string? Website { get; set; }

        [BindProperty]
        public string? AccessPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Submitted { get; set; }

        public async Task OnGetAsync()
        {
            await LoadSettingsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadSettingsAsync();

            // Anti-bot honeypot
            if (!string.IsNullOrWhiteSpace(Website))
            {
                return RedirectToPage(new { submitted = "OK" });
            }

            ModelState.Remove("Input.Status");
            ModelState.Remove("Input.ReferenceNumber");

            if (string.IsNullOrWhiteSpace(Input.FullName))
                ModelState.AddModelError("Input.FullName", "Full name is required.");
            if (string.IsNullOrWhiteSpace(Input.PhoneNumber))
                ModelState.AddModelError("Input.PhoneNumber", "Phone number is required.");
            if (!Input.ConsentGiven)
                ModelState.AddModelError("Input.ConsentGiven", "You must accept the declaration and consent to submit.");

            if (PassportPhoto != null && PassportPhoto.Length > 0)
            {
                var ext = Path.GetExtension(PassportPhoto.FileName).ToLowerInvariant();
                if (!AllowedPhotoExtensions.Contains(ext))
                    ModelState.AddModelError(nameof(PassportPhoto), "Photo must be a JPG, PNG or WEBP image.");
                else if (PassportPhoto.Length > MaxPhotoBytes)
                    ModelState.AddModelError(nameof(PassportPhoto), "Photo must not be larger than 2 MB.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            MisCommunityFormSubmission target;

            // Check if completing an existing draft
            if (!string.IsNullOrEmpty(Input.ReferenceNumber))
            {
                target = await _context.MisCommunityFormSubmissions
                    .FirstOrDefaultAsync(x => x.ReferenceNumber == Input.ReferenceNumber) ?? new MisCommunityFormSubmission();
            }
            else
            {
                target = new MisCommunityFormSubmission();
                _context.MisCommunityFormSubmissions.Add(target);
            }

            // Copy all fields
            target.FullName = Input.FullName;
            target.PreferredName = Input.PreferredName;
            target.Gender = Input.Gender;
            target.DateOfBirth = Input.DateOfBirth;
            target.MaritalStatus = Input.MaritalStatus;
            target.PhoneNumber = Input.PhoneNumber;
            target.Email = Input.Email;
            target.ResidentialAddress = Input.ResidentialAddress;
            target.StateCountryOfResidence = Input.StateCountryOfResidence;

            target.FatherFullName = Input.FatherFullName;
            target.MotherFullName = Input.MotherFullName;
            target.FamilyKindredName = Input.FamilyKindredName;
            target.ClanName = Input.ClanName;
            target.SpouseName = Input.SpouseName;
            target.NumberOfChildren = Input.NumberOfChildren;
            target.ChildrenNames = Input.ChildrenNames;
            target.RelationshipToCommunity = Input.RelationshipToCommunity;

            target.FatherVillageTown = Input.FatherVillageTown;
            target.MotherVillageTown = Input.MotherVillageTown;
            target.FamilyCompound = Input.FamilyCompound;
            target.KindredLineage = Input.KindredLineage;
            target.FamilyHeadName = Input.FamilyHeadName;
            target.AncestralHome = Input.AncestralHome;
            target.FamilyHistory = Input.FamilyHistory;
            target.RelatedFamilies = Input.RelatedFamilies;

            target.PaternalGrandfather = Input.PaternalGrandfather;
            target.PaternalGrandmother = Input.PaternalGrandmother;
            target.MaternalGrandfather = Input.MaternalGrandfather;
            target.MaternalGrandmother = Input.MaternalGrandmother;
            target.GreatGrandparents = Input.GreatGrandparents;

            target.HighestQualification = Input.HighestQualification;
            target.Institution = Input.Institution;
            target.FieldOfStudy = Input.FieldOfStudy;
            target.ProfessionalCertifications = Input.ProfessionalCertifications;
            target.CurrentCourseOfStudy = Input.CurrentCourseOfStudy;

            target.CurrentOccupation = Input.CurrentOccupation;
            target.Employer = Input.Employer;
            target.JobTitle = Input.JobTitle;
            target.BusinessName = Input.BusinessName;
            target.BusinessType = Input.BusinessType;
            target.BusinessLocation = Input.BusinessLocation;
            target.YearsOfExperience = Input.YearsOfExperience;
            target.ProfessionalSkills = Input.ProfessionalSkills;

            var expertise = (SelectedExpertise ?? new List<string>())
                .Where(e => ExpertiseOptions.Contains(e))
                .Distinct()
                .ToList();
            target.ExpertiseAreas = expertise.Any() ? string.Join(", ", expertise) : null;
            target.OtherExpertise = Input.OtherExpertise;
            target.WillingToOfferSkills = Input.WillingToOfferSkills;

            target.CurrentCommunityPosition = Input.CurrentCommunityPosition;
            target.PreviousLeadershipPositions = Input.PreviousLeadershipPositions;
            target.CommunityAssociations = Input.CommunityAssociations;
            target.CommunityContributions = Input.CommunityContributions;
            target.ContributionInterests = Input.ContributionInterests;
            target.VolunteerInterests = Input.VolunteerInterests;

            target.AlternativePhone = Input.AlternativePhone;
            target.EmergencyContactName = Input.EmergencyContactName;
            target.EmergencyContactPhone = Input.EmergencyContactPhone;
            target.EmergencyContactRelationship = Input.EmergencyContactRelationship;

            target.SocialProfileLink = Input.SocialProfileLink;
            target.Hobbies = Input.Hobbies;
            target.LanguagesSpoken = Input.LanguagesSpoken;

            target.ConsentGiven = true;
            target.ConsentDateUtc = DateTime.UtcNow;
            target.IsDraft = false;
            target.Status = "Pending";
            target.SubmittedFromIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            target.LastSavedAtUtc = DateTime.UtcNow;

            if (string.IsNullOrEmpty(target.ReferenceNumber) || target.ReferenceNumber.StartsWith("DRAFT-"))
            {
                target.ReferenceNumber = $"CF-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
            }

            if (!string.IsNullOrWhiteSpace(AccessPassword))
            {
                target.AccessPinHash = HashPin(AccessPassword);
            }

            if (PassportPhoto != null && PassportPhoto.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads", "community-form");
                Directory.CreateDirectory(folder);
                var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(PassportPhoto.FileName).ToLowerInvariant()}";
                await using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                {
                    await PassportPhoto.CopyToAsync(stream);
                }
                target.PassportPhotoUrl = $"/uploads/community-form/{fileName}";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage(new { submitted = target.ReferenceNumber });
        }

        #region AJAX Real-Time Draft Save & Resume Handlers

        public class DraftSavePayload
        {
            public string? ReferenceNumber { get; set; }
            public string? Password { get; set; }
            public int Step { get; set; } = 1;
            public MisCommunityFormSubmission? Data { get; set; }
            public List<string>? SelectedExpertise { get; set; }
        }

        public async Task<IActionResult> OnPostSaveDraftAsync([FromBody] DraftSavePayload payload)
        {
            if (payload?.Data == null)
            {
                return new JsonResult(new { success = false, message = "Invalid data payload." });
            }

            try
            {
                MisCommunityFormSubmission draft;

                if (!string.IsNullOrEmpty(payload.ReferenceNumber))
                {
                    draft = await _context.MisCommunityFormSubmissions
                        .FirstOrDefaultAsync(x => x.ReferenceNumber == payload.ReferenceNumber);

                    if (draft == null)
                    {
                        draft = new MisCommunityFormSubmission();
                        _context.MisCommunityFormSubmissions.Add(draft);
                    }
                    else if (!string.IsNullOrEmpty(draft.AccessPinHash) && !string.IsNullOrEmpty(payload.Password))
                    {
                        if (draft.AccessPinHash != HashPin(payload.Password))
                        {
                            return new JsonResult(new { success = false, message = "Invalid password for this draft." });
                        }
                    }
                }
                else
                {
                    // Look up existing draft by phone number if provided
                    var phone = payload.Data.PhoneNumber?.Trim();
                    if (!string.IsNullOrEmpty(phone))
                    {
                        draft = await _context.MisCommunityFormSubmissions
                            .FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.IsDraft);
                    }
                    else
                    {
                        draft = null;
                    }

                    if (draft == null)
                    {
                        draft = new MisCommunityFormSubmission();
                        _context.MisCommunityFormSubmissions.Add(draft);
                    }
                }

                // Copy draft fields
                var d = payload.Data;
                draft.FullName = d.FullName ?? "Draft Applicant";
                draft.PreferredName = d.PreferredName;
                draft.Gender = d.Gender;
                draft.DateOfBirth = d.DateOfBirth;
                draft.MaritalStatus = d.MaritalStatus;
                draft.PhoneNumber = d.PhoneNumber ?? "";
                draft.Email = d.Email;
                draft.ResidentialAddress = d.ResidentialAddress;
                draft.StateCountryOfResidence = d.StateCountryOfResidence;

                draft.FatherFullName = d.FatherFullName;
                draft.MotherFullName = d.MotherFullName;
                draft.FamilyKindredName = d.FamilyKindredName;
                draft.ClanName = d.ClanName;
                draft.SpouseName = d.SpouseName;
                draft.NumberOfChildren = d.NumberOfChildren;
                draft.ChildrenNames = d.ChildrenNames;
                draft.RelationshipToCommunity = d.RelationshipToCommunity;

                draft.FatherVillageTown = d.FatherVillageTown;
                draft.MotherVillageTown = d.MotherVillageTown;
                draft.FamilyCompound = d.FamilyCompound;
                draft.KindredLineage = d.KindredLineage;
                draft.FamilyHeadName = d.FamilyHeadName;
                draft.AncestralHome = d.AncestralHome;
                draft.FamilyHistory = d.FamilyHistory;
                draft.RelatedFamilies = d.RelatedFamilies;

                draft.PaternalGrandfather = d.PaternalGrandfather;
                draft.PaternalGrandmother = d.PaternalGrandmother;
                draft.MaternalGrandfather = d.MaternalGrandfather;
                draft.MaternalGrandmother = d.MaternalGrandmother;
                draft.GreatGrandparents = d.GreatGrandparents;

                draft.HighestQualification = d.HighestQualification;
                draft.Institution = d.Institution;
                draft.FieldOfStudy = d.FieldOfStudy;
                draft.ProfessionalCertifications = d.ProfessionalCertifications;
                draft.CurrentCourseOfStudy = d.CurrentCourseOfStudy;

                draft.CurrentOccupation = d.CurrentOccupation;
                draft.Employer = d.Employer;
                draft.JobTitle = d.JobTitle;
                draft.BusinessName = d.BusinessName;
                draft.BusinessType = d.BusinessType;
                draft.BusinessLocation = d.BusinessLocation;
                draft.YearsOfExperience = d.YearsOfExperience;
                draft.ProfessionalSkills = d.ProfessionalSkills;

                if (payload.SelectedExpertise != null && payload.SelectedExpertise.Any())
                {
                    draft.ExpertiseAreas = string.Join(", ", payload.SelectedExpertise);
                }
                else
                {
                    draft.ExpertiseAreas = d.ExpertiseAreas;
                }
                draft.OtherExpertise = d.OtherExpertise;
                draft.WillingToOfferSkills = d.WillingToOfferSkills;

                draft.CurrentCommunityPosition = d.CurrentCommunityPosition;
                draft.PreviousLeadershipPositions = d.PreviousLeadershipPositions;
                draft.CommunityAssociations = d.CommunityAssociations;
                draft.CommunityContributions = d.CommunityContributions;
                draft.ContributionInterests = d.ContributionInterests;
                draft.VolunteerInterests = d.VolunteerInterests;

                draft.AlternativePhone = d.AlternativePhone;
                draft.EmergencyContactName = d.EmergencyContactName;
                draft.EmergencyContactPhone = d.EmergencyContactPhone;
                draft.EmergencyContactRelationship = d.EmergencyContactRelationship;

                draft.SocialProfileLink = d.SocialProfileLink;
                draft.Hobbies = d.Hobbies;
                draft.LanguagesSpoken = d.LanguagesSpoken;

                draft.IsDraft = true;
                draft.LastActiveStep = payload.Step > 0 ? payload.Step : 1;
                draft.LastSavedAtUtc = DateTime.UtcNow;

                if (!string.IsNullOrWhiteSpace(payload.Password))
                {
                    draft.AccessPinHash = HashPin(payload.Password);
                }

                if (string.IsNullOrEmpty(draft.ReferenceNumber))
                {
                    draft.ReferenceNumber = $"DRAFT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
                }

                await _context.SaveChangesAsync();

                return new JsonResult(new
                {
                    success = true,
                    referenceNumber = draft.ReferenceNumber,
                    lastSaved = draft.LastSavedAtUtc?.ToString("hh:mm:ss tt"),
                    step = draft.LastActiveStep,
                    message = "Draft saved successfully."
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Failed to save draft: " + ex.Message });
            }
        }

        public class DraftResumePayload
        {
            public string Identifier { get; set; } = null!;
            public string? Password { get; set; }
        }

        public async Task<IActionResult> OnPostResumeDraftAsync([FromBody] DraftResumePayload payload)
        {
            if (string.IsNullOrWhiteSpace(payload?.Identifier))
            {
                return new JsonResult(new { success = false, message = "Please enter your Reference Code, Phone Number, or Email." });
            }

            var term = payload.Identifier.Trim();
            var draft = await _context.MisCommunityFormSubmissions
                .Where(x => x.ReferenceNumber == term || x.PhoneNumber == term || x.Email == term)
                .OrderByDescending(x => x.LastSavedAtUtc ?? x.CreatedAtUtc)
                .FirstOrDefaultAsync();

            if (draft == null)
            {
                return new JsonResult(new { success = false, message = "No saved application found matching that code, phone, or email." });
            }

            // Check PIN if draft was password-protected
            if (!string.IsNullOrEmpty(draft.AccessPinHash))
            {
                if (string.IsNullOrEmpty(payload.Password) || HashPin(payload.Password) != draft.AccessPinHash)
                {
                    return new JsonResult(new { success = false, requiresPassword = true, message = "This draft is protected with a password/PIN. Please provide the correct password to continue." });
                }
            }

            var expertiseList = !string.IsNullOrEmpty(draft.ExpertiseAreas)
                ? draft.ExpertiseAreas.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
                : new List<string>();

            return new JsonResult(new
            {
                success = true,
                referenceNumber = draft.ReferenceNumber,
                step = draft.LastActiveStep,
                lastSaved = (draft.LastSavedAtUtc ?? draft.CreatedAtUtc).ToString("MMM dd, yyyy hh:mm tt"),
                isDraft = draft.IsDraft,
                data = draft,
                selectedExpertise = expertiseList,
                message = $"Draft loaded successfully from {draft.LastSavedAtUtc?.ToString("MMM dd, yyyy hh:mm tt") ?? "your previous session"}."
            });
        }

        #endregion

        private static string HashPin(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin)) return "";
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pin.Trim()));
            return Convert.ToBase64String(bytes);
        }

        private async Task LoadSettingsAsync()
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync() ?? new SuperSetting();
            OrgName = SuperSetting.CompanyName ?? OrgName;
        }
    }
}
