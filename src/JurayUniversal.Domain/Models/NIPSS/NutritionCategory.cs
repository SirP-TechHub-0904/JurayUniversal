using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models.NIPSS
{
    public class NutritionCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Disable { get;set;}

        public ICollection<NutritionCategoryList> NutritionCategoryList { get; set; }
        public ICollection<UserNutritionEvaluation> UserNutritionEvaluation { get; set; }
    }
}
