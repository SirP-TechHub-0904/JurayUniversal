using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class Salary
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Profile User { get; set; }
        public decimal Amount { get; set; }
        public decimal DefaulterCharge { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal Balance { get; set; }
        public string Note { get; set; }
        
        public string Month { get; set; }
        public FundStatus PaymentStatus {get; set; }

        

        public bool ApprovedByManager { get; set; }
        public string ManagerNote { get; set; }
        public DateTime ApprovedByManagerDate { get; set; }
        public bool ApprovedByCEO { get; set; }
        public string CeoNote { get; set; }
        public DateTime ApprovedByCeoDate { get; set; }

        public bool ApprovedByCFO { get; set; }
        public string CfoNote { get; set; }
        public DateTime ApprovedByCfoDate { get; set; }
    }
}
