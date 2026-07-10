using System;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisSmsRecipient
    {
        public long Id { get; set; }
        
        public long MisSmsCampaignId { get; set; }
        public MisSmsCampaign? MisSmsCampaign { get; set; }
        
        public string? PhoneNumber { get; set; }
        
        public MisSmsDeliveryStatus DeliveryStatus { get; set; }
        
        public string? Reference { get; set; }
    }
}
