using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.DomainServices
{
    public class DomainDataDto
    {
        public long Id { get;set; }
        public string Domain { get;set;}
        public string BaseCompanyUrl { get;set;}
        public DateTime Date { get;set;}
    }
}
