using System;
using System.Collections.Generic;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisSmsCampaign
    {
        public long Id { get; set; }
        
        public string? UserId { get; set; }
        public Profile? User { get; set; }
        
        public string? SenderId { get; set; }
        
        public string? Subject { get; set; }
        public string? Message { get; set; }
        
        public decimal CostIncurred { get; set; }
        public int TotalRecipients { get; set; }
        
        public MisSmsCampaignStatus Status { get; set; }
        
        public DateTime? ScheduledDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MisSmsRecipient> Recipients { get; set; } = new List<MisSmsRecipient>();
    }
}
