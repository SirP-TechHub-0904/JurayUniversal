using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class Course
    {
        public long Id { get; set; }


        public long? CourseCategoryId { get; set; }
        [Display(Name = "Course Category")]
        public CourseCategory CourseCategory { get; set; }
        [Display(Name = "SEC Number")]
        public string SecNumber { get; set; }


        public string? Description { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public DateTime Year { get; set; }
        [Display(Name = "Course Status")]
        public CourseStatus CourseStatus { get; set; }
        public string Theme { get; set; }
        [Display(Name = "Sub Themes")]
        public ICollection<CourseSubTheme> SubThemes { get; set; }

        public ICollection<ParticipantList> ParticipantLists { get; set; }

        public ICollection<StudyGroupCategory> StudyGroupCategories { get; set; }
    }
}
