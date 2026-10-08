using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Testimonials
{
    public class TestimonialItemDto
    {
        public int Id { get; set; }
        public string RepresentativeName { get; set; } = string.Empty;
        public string PositionAndOrg { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string Quote { get; set; } = string.Empty;
        public string AvatarPath { get; set; } = string.Empty;
        public int Rating { get; set; } = 5;
        public bool IsPublished { get; set; } = true;
    }

    public class IndexModel : PageModel
    {
        public List<TestimonialItemDto> Testimonials { get; set; } = new();

        public void OnGet()
        {
            Testimonials = new List<TestimonialItemDto>
            {
                new TestimonialItemDto
                {
                    Id = 1,
                    RepresentativeName = "Dr. Ibrahim Usman",
                    PositionAndOrg = "Director of Health Records & ICT, Federal Ministry of Health",
                    Headline = "Seamless Staff & Health Records",
                    Quote = "Managing staff postings, clinic schedules, and institutional records across multiple departments is now seamless and completely stress-free.",
                    AvatarPath = "/xyz-mis/assets/img/images/testi_avatar01.jpg",
                    Rating = 5,
                    IsPublished = true
                },
                new TestimonialItemDto
                {
                    Id = 2,
                    RepresentativeName = "Prof. Anthony Okafor",
                    PositionAndOrg = "Head of Academic Operations, NIPSS Kuru",
                    Headline = "Smooth Student & Course Portals",
                    Quote = "Participant registration, lecture schedules, and result collation run effortlessly on Exwhyzee MIS without technical hitches or delays.",
                    AvatarPath = "/xyz-mis/assets/img/images/testi_avatar02.jpg",
                    Rating = 5,
                    IsPublished = true
                },
                new TestimonialItemDto
                {
                    Id = 3,
                    RepresentativeName = "Hajia Maryam Bello",
                    PositionAndOrg = "Planning & Operations Lead, Ministry of Budget & Economic Planning",
                    Headline = "Clear Project Tracking & Reports",
                    Quote = "The automated reporting and departmental workflow tools have given our management total clarity and timely execution on key projects.",
                    AvatarPath = "/xyz-mis/assets/img/images/testi_avatar03.jpg",
                    Rating = 5,
                    IsPublished = true
                },
                new TestimonialItemDto
                {
                    Id = 4,
                    RepresentativeName = "Chief Kingsley Anyanwu",
                    PositionAndOrg = "General Secretary, Urualla Community Development",
                    Headline = "Transparent Online Voting",
                    Quote = "Over 15,000 members voted in our annual elections online from their mobile phones. It was 100% transparent, fast, and results were instant.",
                    AvatarPath = "/xyz-mis/assets/img/images/testi_avatar04.jpg",
                    Rating = 5,
                    IsPublished = true
                },
                new TestimonialItemDto
                {
                    Id = 5,
                    RepresentativeName = "Joy Adetola",
                    PositionAndOrg = "Head of Payroll & Accounts, Ekefin Financial Services",
                    Headline = "Automated Invoicing & Payroll",
                    Quote = "Monthly salary calculations, customer invoicing, and financial reconciliation that used to take days now happen automatically in minutes.",
                    AvatarPath = "/xyz-mis/assets/img/images/testi_avatar05.jpg",
                    Rating = 5,
                    IsPublished = true
                },
                new TestimonialItemDto
                {
                    Id = 6,
                    RepresentativeName = "Engr. Emeka Nwankwo",
                    PositionAndOrg = "Lead Solutions Engineer, Juray Technologies",
                    Headline = "100% Safe, Private & Reliable",
                    Quote = "Every school and business gets their own dedicated, private environment with fast loading speeds and reliable 24/7 technical support.",
                    AvatarPath = "/xyz-mis/assets/img/images/testi_avatar06.jpg",
                    Rating = 5,
                    IsPublished = true
                }
            };
        }
    }
}
