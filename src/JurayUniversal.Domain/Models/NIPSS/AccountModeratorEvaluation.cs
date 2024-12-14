using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class AccountModeratorEvaluation
    {
        public long Id { get; set; }
        public long? ModeratorId { get; set; }
        public Moderator Moderator { get; set; }

        public long? TimeTableId { get; set; }
        public TimeTable TimeTable { get; set; }


        public int? ParticipantListId { get; set; }
        public ParticipantList ParticipantList { get; set; }

        public int Response { get; set; }

        public long? EvaluationQuestionId { get; set; }
        public EvaluationQuestion EvaluationQuestion { get; set; }
    }
}
