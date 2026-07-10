using System;
using System.Collections.Generic;

namespace JurayUniversal.Domain.Models
{
    public class MisWallet
    {
        public long Id { get; set; }
        
        public string? UserId { get; set; }
        public Profile? User { get; set; }
        
        public decimal Balance { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public ICollection<MisWalletTransaction> Transactions { get; set; } = new List<MisWalletTransaction>();
    }
}
