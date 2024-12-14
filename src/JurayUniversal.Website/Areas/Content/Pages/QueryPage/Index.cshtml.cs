using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace JurayUniversal.Website.Areas.Content.Pages.QueryPage
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IQueryable<QueryDto> QueryDtos { get; set; }

        [BindProperty]
        public string Searchfile { get; set; }

        [BindProperty]
        public string Fields { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Searchfile = "Seminary Ahiaeke";
            if (!string.IsNullOrEmpty(Searchfile))
            {
                //string searchQuery = Request.Query[Searchfile];
                //string[] keywords = searchQuery.Split(' ');

                string input = "and Chairman of EXWHYZEE was born on 15th May, 1976 to the Engr. Ifeanyi Jude Seminary Ahiaeke Ngama, Founder and Chairman of EXWHYZEE was born on 15th May, 1976 to the family of late Sir Everist Atasie Ngama and Lady Dorathy Chinyere Ngama Seminary Ahiaeke of Alaike Ozuomee Urualla in Ideato North LGA, Imo State Nigeria. He started his formal education in 1982 at Ibeama Community Primary School Umuana Ibeku, Umuahia Seminary Ahiaeke and did his Secondary School at Immaculate Conception Dorathy Seminary Ahiaeke, Umuahia and finished in 1993. Thereafter he gained admission into Seminary Ahiaeke Federal University of Technology Owerri (FUTO) and graduated in 2001 with B.Eng. in Mechanical Dorathy Engineering and also did his Seminary Ahiaeke MBA in Project Management in 2006. His National Youth Service Corps (NYSC) was in Numan, Adamawa State where he served in Bright Future Seminary Ahiaeke International School Seminary Ahiaeke and won the ";

                Fields = HighlightText(input, Searchfile);

                string[] words = Regex.Split(Fields, @"\s+|\.|,|!|\?|<b>|</b>");

                string[] searchwords = Regex.Split(Searchfile, @"\s+|\.|,|!|\?|<b>|</b>");
                //string[] words = Fields.Split(new[] { ' ', '.', ',', '!', '?', '<b>', '</b>' }, StringSplitOptions.RemoveEmptyEntries);

                string qas = searchwords.FirstOrDefault();
                // Find the indices of the keyword occurrences
                List<int> keywordIndices = new List<int>();
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Equals(qas, StringComparison.OrdinalIgnoreCase))
                    {
                        keywordIndices.Add(i);
                    }
                }

                // Extract four words after each keyword occurrence
                List<string> extractedWords = new List<string>();
                List<int> resultindices = new List<int>();
                foreach (int index in keywordIndices)
                {
                    int xstartIndex = index;
                    //for()
                    //int xendIndex = Math.Min(xstartIndex + 4, words.Length);
                    //string[] extracted = new string[xendIndex - xstartIndex];
                    //Array.Copy(words, xstartIndex, extracted, 0, extracted.Length);
                    //extractedWords.Add(string.Join(" ", extracted));
                    //resultindices.Add();
                }

                // Display the extracted words
                foreach (string extracted in extractedWords)
                {
                    Console.WriteLine(extracted);
                }



                // Assuming you have a data context named "dbContext" and an entity named "Item"
                //IQueryable<WebPage> items = _context.WebPages.Where(item => keywords.Any(keyword =>
                //    item.Title.Contains(keyword))).AsQueryable();

            }


            return Page();
        }
        public static string GetWordsByIndex(List<string> words, List<int> indices)
        {
            var selectedWords = indices.Select(i => words[i]);
            return string.Join(" ", selectedWords);
        }
        static string HighlightText(string input, string searchText)
        {
            //string pattern = $@"\b{Regex.Escape(searchText)}\b";
            string highlightStartTag = "<b>";
            string highlightEndTag = "</b>";

            // Find all occurrences of the search text
            MatchCollection matches = Regex.Matches(input, searchText, RegexOptions.IgnoreCase);

            // Insert highlight tags around the search text
            int offset = 0;
            foreach (Match match in matches)
            {
                int startIndex = match.Index + offset;
                int endIndex = match.Index + match.Length + offset;

                // Insert opening highlight tag
                input = input.Insert(startIndex, highlightStartTag);
                //offset += highlightStartTag.Length;

                // Insert closing highlight tag
                input = input.Insert(endIndex + 3, highlightEndTag);
                //offset += highlightEndTag.Length;

                // Split the data into an array of words
                



                offset += 7;
            }

            return input;
        }
        public string HighlightSearchQuery(string value, string[] keywords)
        {
            foreach (string keyword in keywords)
            {
                value = value.Replace(keyword, $"<strong>{keyword}</strong>");
            }

            return value;
        }
    }

    public class QueryDto
    {
        public long Qid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string XArea { get; set; }
        public string XFolder { get; set; }
        public string XPage { get; set; }
    }
}
