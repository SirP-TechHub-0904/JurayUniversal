using System;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisSmsSenderId
    {
        public long Id { get; set; }
        
        public string? SenderId { get; set; }
        
        public string? SampleMessage { get; set; }
        
        public MisSenderIdStatus Status { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
