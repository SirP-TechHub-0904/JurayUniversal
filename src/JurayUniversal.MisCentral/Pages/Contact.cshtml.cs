using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IProspectiveLeadService _leadService;

        public ContactModel(IProspectiveLeadService leadService)
        {
            _leadService = leadService;
        }

        [BindProperty]
        public ContactInputModel Input { get; set; } = new ContactInputModel();

        public bool IsSubmitted { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet(string? org, string? plan, string? feature)
        {
            if (!string.IsNullOrEmpty(org))
            {
                Input.OrganizationName = org;
            }

            if (!string.IsNullOrEmpty(plan))
            {
                Input.PrimaryGoal = $"We are interested in adopting the {plan.ToUpperInvariant()} plan.";
            }
            else if (!string.IsNullOrEmpty(feature))
            {
                Input.SelectedFeatures.Add(feature);
                Input.PrimaryGoal = $"We would like to activate the {feature} feature.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var lead = new ProspectiveLead
            {
                OrganizationName = Input.OrganizationName,
                OrganizationType = Input.OrganizationType,
                EstimatedSize = Input.EstimatedSize,
                StateOrCity = Input.StateOrCity,
                ContactPersonName = Input.FullName,
                JobTitle = Input.JobTitle,
                Email = Input.Email,
                Phone = Input.Phone,
                FeaturesOfInterest = Input.SelectedFeatures,
                PrimaryGoal = Input.PrimaryGoal,
                RolloutTimeline = Input.RolloutTimeline,
                AdditionalNotes = Input.AdditionalNotes,
                Status = "New",
                CreatedAtUtc = DateTime.UtcNow
            };

            await _leadService.CreateLeadAsync(lead);

            IsSubmitted = true;
            SuccessMessage = $"Thank you, {Input.FullName}! Your demonstration and onboarding request for {Input.OrganizationName} has been received. Our team will contact you shortly.";
            return Page();
        }

        public class ContactInputModel
        {
            [Required(ErrorMessage = "Please enter your full name")]
            [Display(Name = "Contact Representative")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter your job role/position")]
            [Display(Name = "Job Title / Role")]
            public string JobTitle { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter your email")]
            [EmailAddress]
            [Display(Name = "Official Email Address")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter your phone/WhatsApp number")]
            [Display(Name = "Phone / WhatsApp Number")]
            public string Phone { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please enter your organization name")]
            [Display(Name = "Organization / Institution Name")]
            public string OrganizationName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Please select your organization type")]
            [Display(Name = "Organization Category")]
            public string OrganizationType { get; set; } = "Primary / Secondary School";

            [Display(Name = "Estimated Users / Students / Staff")]
            public string EstimatedSize { get; set; } = "250 - 1,000 Users";

            [Display(Name = "State / City")]
            public string StateOrCity { get; set; } = string.Empty;

            [Display(Name = "Features of Interest")]
            public List<string> SelectedFeatures { get; set; } = new();

            [Required(ErrorMessage = "Please tell us your primary operational goal")]
            [Display(Name = "Main Operational Goals / Challenges to Solve")]
            public string PrimaryGoal { get; set; } = string.Empty;

            [Display(Name = "Expected Rollout Timeline")]
            public string RolloutTimeline { get; set; } = "Within 30 Days";

            [Display(Name = "Additional Notes / Specific Questions")]
            public string AdditionalNotes { get; set; } = string.Empty;
        }
    }
}
