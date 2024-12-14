using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class ProgramUser
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public Profile User {  get; set; }

        public long? CompanyProgramId {  get; set; }
        public CompanyProgram CompanyProgram { get; set; }
    }
}
