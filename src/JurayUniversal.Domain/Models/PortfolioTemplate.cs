using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class PortfolioTemplate
    {
        public long Id { get;set;}
        public string Name { get;set;}
        public string? Path { get;set;}
        public string? SampleUrl { get;set;}
        public string? SampleKey { get;set;}
    }
}
