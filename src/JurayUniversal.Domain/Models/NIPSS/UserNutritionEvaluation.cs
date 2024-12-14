using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class UserNutritionEvaluation
    {
        public long Id { get; set; }

        public long? NutritionCategoryId { get; set; }
        public NutritionCategory NutritionCategory { get; set; }

        public long NutritionCategoryListId { get; set; }
        public NutritionCategoryList NutritionCategoryList { get; set; }
         
        public ResultType ResultType { get; set; }
        public string? Result { get; set; }

        public string? UserId { get; set; }
        public Profile User { get; set; }

    }
}
