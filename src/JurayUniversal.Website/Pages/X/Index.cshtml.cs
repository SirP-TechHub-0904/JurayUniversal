using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace JurayUniversal.Website.Pages.X
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
        public JsonResult OnGetAAA()
        {
            // Fetch the data
            var list = new List<Dresult>();

            // Populate the list with data
            list.Add(new Dresult { State = "State 1", Population = "1000" });
            list.Add(new Dresult { State = "State 2", Population = "2000" });
            list.Add(new Dresult { State = "State 3", Population = "3000" });

            return new JsonResult(list);

        }
    }

    public class Dresult
    {
        public string State { get; set; }
        public string Population { get; set; }
        public string Color { get; set; }
    }
}
