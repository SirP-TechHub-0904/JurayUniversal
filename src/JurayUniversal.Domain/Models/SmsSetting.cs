using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class SmsSetting
    {
        public SmsSetting()
        {
            UnitPerNewMember = 50;
            DefaultUserUnitReorderLevel = 200;
            PricePerUnit = 2;

            SendAccountCreditedNotification = true;
            SendLowOnUnitsNotification = true;
            SendOrderApprovedNotification = true;
            SendRecievedRequestNotification = true;
            SendReminderToDebtor = false;
            SendUserBirthdayWishes = true;
            PreventApiModification = false;
        }

        public long Id { get; set; }
        public decimal UnitPerNewMember { get; set; }
        public decimal DefaultUserUnitReorderLevel { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal FlatUnitsPerSms { get; set; }
        public string BlackListedWords { get; set; }

        public bool SendOrderApprovedNotification { get; set; }
        public bool SendLowOnUnitsNotification { get; set; }
        public bool SendRecievedRequestNotification { get; set; }
        public bool SendReminderToDebtor { get; set; }
        public bool SendAccountCreditedNotification { get; set; }
        public bool SendUserBirthdayWishes { get; set; }
        public bool PreventApiModification { get; set; }
    }
}
