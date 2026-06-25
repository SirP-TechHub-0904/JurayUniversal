using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisForms.Pages.Dashboard
{
    [Authorize]
    public class ResponsesModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ResponsesModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisForm Form { get; set; }
        public IList<MisFormResponse> FormResponses { get; set; }

        public List<string> HeaderFields { get; set; } = new List<string>();
        public Dictionary<string, string> FieldLabels { get; set; } = new Dictionary<string, string>();

        public async Task<IActionResult> OnGetAsync(long id)
        {
            Form = await _context.MisForms
                .Include(f => f.Responses)
                .ThenInclude(r => r.UserProfile)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Form == null)
            {
                return NotFound();
            }

            FormResponses = Form.Responses.OrderByDescending(r => r.SubmittedAt).ToList();

            // Attempt to parse headers from the schema
            try
            {
                if (!string.IsNullOrEmpty(Form.SchemaJson) && Form.SchemaJson != "[]")
                {
                    var schemaArray = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(Form.SchemaJson);
                    foreach (var field in schemaArray)
                    {
                        if (field.TryGetValue("name", out var nameObj))
                        {
                            var nameStr = nameObj.ToString();
                            if (field.TryGetValue("label", out var labelObj))
                            {
                                var labelStr = labelObj.ToString();
                                HeaderFields.Add(nameStr);
                                FieldLabels[nameStr] = labelStr;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore schema parse errors for headers
            }

            return Page();
        }
    }
}
