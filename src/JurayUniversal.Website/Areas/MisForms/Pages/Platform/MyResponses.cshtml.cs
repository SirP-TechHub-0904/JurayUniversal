using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisForms.Pages.Platform
{
    [Authorize]
    public class MyResponsesModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public MyResponsesModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MisForm Form { get; set; }
        public IList<MisFormResponse> FormResponses { get; set; }

        public List<string> HeaderFields { get; set; } = new List<string>();
        public Dictionary<string, string> FieldLabels { get; set; } = new Dictionary<string, string>();
        
        // Maps a Response Id to its Parsed Key-Value Answers
        public Dictionary<long, Dictionary<string, string>> ParsedAnswers { get; set; } = new Dictionary<long, Dictionary<string, string>>();

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var userId = _userManager.GetUserId(User);

            Form = await _context.MisForms
                .Include(f => f.Responses.Where(r => r.UserProfileId == userId))
                .FirstOrDefaultAsync(m => m.Id == id && m.IsPublished);

            if (Form == null || !Form.AllowUserToViewResponses)
            {
                return NotFound();
            }

            FormResponses = Form.Responses.OrderByDescending(r => r.SubmittedAt).ToList();

            // 1. Parse Schema Headers
            try
            {
                if (!string.IsNullOrEmpty(Form.SchemaJson) && Form.SchemaJson != "[]")
                {
                    using var schemaDoc = JsonDocument.Parse(Form.SchemaJson);
                    foreach (var element in schemaDoc.RootElement.EnumerateArray())
                    {
                        var name = element.GetProperty("name").GetString();
                        var label = element.GetProperty("label").GetString();
                        
                        // Ignore non-input elements
                        var type = element.GetProperty("type").GetString();
                        if (type == "header" || type == "paragraph") continue;

                        HeaderFields.Add(name);
                        FieldLabels[name] = label;
                    }
                }
            }
            catch { }

            // 2. Parse Actual Responses
            foreach (var response in FormResponses)
            {
                var answers = new Dictionary<string, string>();
                try
                {
                    if (!string.IsNullOrEmpty(response.ResponsesJson))
                    {
                        using var resDoc = JsonDocument.Parse(response.ResponsesJson);
                        foreach (var kvp in resDoc.RootElement.EnumerateObject())
                        {
                            var val = kvp.Value.ValueKind == JsonValueKind.Array
                                ? string.Join(", ", kvp.Value.EnumerateArray().Select(v => v.GetString()))
                                : kvp.Value.GetString();
                            
                            answers[kvp.Name] = val;
                        }
                    }
                }
                catch { }

                ParsedAnswers[response.Id] = answers;
            }

            return Page();
        }
    }
}
