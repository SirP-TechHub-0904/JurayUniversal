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
        
        // Maps a Response Id to its Parsed Key-Value Answers
        public Dictionary<long, Dictionary<string, string>> ParsedAnswers { get; set; } = new Dictionary<long, Dictionary<string, string>>();

        public class FormFieldStat
        {
            public string Name { get; set; }
            public string Label { get; set; }
            public string Type { get; set; }
            public Dictionary<string, int> AnswerCounts { get; set; } = new Dictionary<string, int>();
        }

        public List<FormFieldStat> Statistics { get; set; } = new List<FormFieldStat>();

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

            // 1. Parse Schema Headers & Identify Stat Fields
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

                            // If this is a chartable field (radio-group, checkbox-group, select)
                            if (field.TryGetValue("type", out var typeObj))
                            {
                                var typeStr = typeObj.ToString();
                                if (typeStr == "checkbox-group" || typeStr == "radio-group" || typeStr == "select")
                                {
                                    var statObj = new FormFieldStat
                                    {
                                        Name = nameStr,
                                        Label = field.TryGetValue("label", out var l) ? l.ToString() : nameStr,
                                        Type = typeStr
                                    };

                                    // Pre-populate AnswerCounts with 0 for all defined options
                                    if (field.TryGetValue("values", out var valuesObj))
                                    {
                                        try
                                        {
                                            var valuesStr = valuesObj.ToString();
                                            var valuesArray = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(valuesStr);
                                            foreach (var valItem in valuesArray)
                                            {
                                                if (valItem.TryGetValue("value", out var vObj))
                                                {
                                                    statObj.AnswerCounts[vObj.ToString()] = 0;
                                                }
                                                else if (valItem.TryGetValue("label", out var lblObj))
                                                {
                                                    statObj.AnswerCounts[lblObj.ToString()] = 0;
                                                }
                                            }
                                        }
                                        catch { }
                                    }

                                    Statistics.Add(statObj);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore schema parse errors
            }

            // 2. Parse User Responses and Tally Statistics
            foreach (var response in FormResponses)
            {
                var answers = new Dictionary<string, string>();
                try
                {
                    if (!string.IsNullOrEmpty(response.ResponsesJson))
                    {
                        var doc = JsonDocument.Parse(response.ResponsesJson);
                        if (doc.RootElement.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var prop in doc.RootElement.EnumerateObject())
                            {
                                string cleanPropName = prop.Name.EndsWith("[]") ? prop.Name.Substring(0, prop.Name.Length - 2) : prop.Name;

                                string valStr = "";
                                if (prop.Value.ValueKind == JsonValueKind.Array)
                                {
                                    var vals = prop.Value.EnumerateArray().Select(x => x.GetString()).Where(x => !string.IsNullOrEmpty(x));
                                    valStr = string.Join(", ", vals);
                                }
                                else
                                {
                                    valStr = prop.Value.GetString();
                                }
                                answers[cleanPropName] = valStr;

                                // Tally stats
                                var statField = Statistics.FirstOrDefault(s => s.Name == cleanPropName);
                                if (statField != null && !string.IsNullOrEmpty(valStr))
                                {
                                    // For checkboxes, values might be comma separated
                                    var splitVals = valStr.Split(new[] { ", " }, System.StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var v in splitVals)
                                    {
                                        if (statField.AnswerCounts.ContainsKey(v))
                                            statField.AnswerCounts[v]++;
                                        else
                                            statField.AnswerCounts[v] = 1;
                                    }
                                }
                            }
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
