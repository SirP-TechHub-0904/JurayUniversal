using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class QuestionAndAnswer
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public Profile User { get; set; }
        public DateTime Date { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }


        public long? ModeratorId { get; set; }
        public Moderator Moderator { get; set; }
    }
}
