using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class EvaluationQuestion
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public string AbbreviatedQuestion { get; set; }
        public bool Publish {  get; set; }

    }
}
