using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{

    public class Profile : IdentityUser
    {
        [Display(Name = "Unique ID")]
        public string? UniqueId { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "User Status")]
        public UserStatus UserStatus { get; set; }

        [Display(Name = "Gender")]
        public GenderStatus Gender { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Permanent Home Address")]
        public string? PermanentHomeAddress { get; set; }

        [Display(Name = "LGA")]
        public string? PermanentLga { get; set; }

        [Display(Name = "State of Origin")]
        public string? PermanentState { get; set; }

        [Display(Name = "Role")]
        public string? Role { get; set; }
        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "State")]
        public string? State { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }


        [Display(Name = "CV")]
        public string? CVFilePathUrl { get; set; }
        public string? CVFilePathKey { get; set; }

        [Display(Name = "Passport Photograph")]
        public string? PassportFilePathUrl { get; set; }
        public string? PassportFilePathKey { get; set; }

        [Display(Name = "Position / Rank")]
        public string? PositionOrRank { get; set; }

        [Display(Name = "Nationality")]
        public string? Nationality { get; set; }

        [Display(Name = "Blood Group")]
        public string? BloodGroup { get; set; }

        [Display(Name = "Genotype")]
        public string? Genotype { get; set; }

        [Display(Name = "Appointment before Resuming at NIPSS")]
        public string? PreviousAppointment { get; set; }

        [Display(Name = "International Passport Number")]
        public string? InternationalPassportNumber { get; set; }

        [Display(Name = "International Passport Expiring Date")]
        public string? InternationalPassportExpiringDate { get; set; }

        [Display(Name = "Marital Status")]
        public string? MaritalStatus { get; set; }


        [Display(Name = "Sponsor")]
        public string? Sponsor { get; set; }
        [Display(Name = "Full Address of Sponsor")]
        public string? SponsorAddress { get; set; }

        [Display(Name = "Emergency Contact Name")]
        public string? EmergencyContactName { get; set; }

        [Display(Name = "Emergency Contact Number")]
        public string? EmergencyContactNumber { get; set; }

        [Display(Name = "Emergency Contact Relationship")]
        public string? EmergencyContactRelationship { get; set; }

        [Display(Name = "Emergency Contact Valid ID Card")]
        public string? EmergencyContactValidIdCardUrl { get; set; }
        public string? EmergencyContactValidIdCardKey { get; set; }

        public string GetFullNameWithTitle()
        {
            string fullName = string.Empty;

            if (!string.IsNullOrEmpty(Title))
            {
                fullName += Title;
            }



            if (!string.IsNullOrEmpty(FirstName))
            {
                fullName += FirstName;
            }

            if (!string.IsNullOrEmpty(MiddleName))
            {
                if (!string.IsNullOrEmpty(fullName))
                {
                    fullName += " ";
                }
                fullName += MiddleName;
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                if (!string.IsNullOrEmpty(fullName))
                {
                    fullName += " ";
                }
                fullName += LastName;
            }

            return fullName;
        }


        public string GetFullName()
        {
            string fullName = string.Empty;

            if (!string.IsNullOrEmpty(FirstName))
            {
                fullName += FirstName;
            }

            if (!string.IsNullOrEmpty(MiddleName))
            {
                if (!string.IsNullOrEmpty(fullName))
                {
                    fullName += " ";
                }
                fullName += MiddleName;
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                if (!string.IsNullOrEmpty(fullName))
                {
                    fullName += " ";
                }
                fullName += LastName;
            }

            return fullName;
        }
        [Display(Name = "FullName")]
        public string FullnameX
        {
            get
            {
                return Title + " " + FirstName + " " + MiddleName + " " + LastName;
            }
        }
        public long? JobDesignationId { get; set; }
        public JobDesignation JobDesignation { get; set; }

        public string? RefCode { get; set; }


        public string? Biography { get; set; }

        [Display(Name = "Alternative Phone Number")]
        public string? AltPhoneNumber { get; set; }
        [Display(Name = "Personal Email")]
        public string? PersonalEmail { get; set; }
        [Display(Name = "Website Address")]
        public string? WebsiteAddress { get; set; }


        [Display(Name = "Date Of Birth Status")]
        public DOBStatus DateOfBirthStatus { get; set; }


        [Display(Name = "Contact Mail Email")]
        public string? ContactMailEmail { get; set; }
        public bool PortfolioProfile { get; set; }


        public long? UserCategoryId { get; set; }
        public UserCategory UserCategory { get; set; }



        [Display(Name = "Referee (2 Referees)")]
        public string? Referee { get; set; }
        [Display(Name = "BVN")]
        public string? BVN { get; set; }

        [Display(Name = "Valid ID Card")]
        public string? ValidIDCardUrl { get; set; }
        public string? ValidIDCardKey { get; set; }

        public bool EmailSent { get; set; }
        public bool SmsSent { get; set; }
        public bool ResetPassword { get; set; }
        public bool UpdateProfile { get; set; }
        public bool UpdateEducation { get; set; }
        public bool UpdateExperience { get; set; }
        public bool UpdateCertificate { get; set; }
        public bool UpdateAwards { get; set; }
        public bool UpdateSkills { get; set; }
        public bool UpdateLanguage { get; set; }
        public bool UpdateInterest { get; set; }
        public bool UpdateReference { get; set; }
        public string? TempPass { get; set; }

        public bool HasAccomodation { get; set; }

        public AccomodationUpdateStatus AccomodationUpdateStatus { get; set; }

        public string? Logs { get; set; }
        public string? LastLogin { get; set; }
    }

}
