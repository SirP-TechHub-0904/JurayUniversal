using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class NutritionCategoryList
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public ResultType ResultType { get; set; }
        public UserNutritionEvaluation UserNutritionEvaluation { get; set; }
        public long? NutritionCategoryId { get; set; }
        public NutritionCategory NutritionCategory { get; set; } 
    }
}
