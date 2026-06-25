using System;
using System.Collections.Generic;

namespace JurayUniversal.Domain.Models
{
    public class MisForm
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortCode { get; set; }
        public string SchemaJson { get; set; }
        public bool IsPublished { get; set; }
        public bool RequireLogin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MisFormResponse> Responses { get; set; }
    }
}
