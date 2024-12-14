 
namespace JurayUniversal.Domain.Models
{
    public class TemplateType
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string? Url { get; set; }
        public string? Key { get; set; }

        public string TrackKey { get; set; } 
    }
}
