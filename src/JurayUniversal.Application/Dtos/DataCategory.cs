using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Dtos
{
    public class DataCategory
    {
        public string Title { get;set;}
        public ICollection<QueryDataList> QueryDataList { get; set; }
    }
}
