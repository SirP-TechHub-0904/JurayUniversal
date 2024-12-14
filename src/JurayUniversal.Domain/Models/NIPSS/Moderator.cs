using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class Moderator
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public Profile User {  get; set; }

        public long? TimeTableId { get; set; }
        public TimeTable TimeTable { get; set; }

        public string? Fullname { get; set; }
        public string? Position { get; set; }

        public ICollection<QuestionAndAnswer> QuestionAndAnswers { get; set; }
        public ICollection<AccountModeratorEvaluation> ModeratorEvaluations { get; set; }
    }
}
