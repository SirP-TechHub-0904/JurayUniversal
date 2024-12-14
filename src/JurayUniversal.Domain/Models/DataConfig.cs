using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class DataConfig
    {
        public long Id { get;set;}
        

        [Display(Name = "Login CSS")]
        public string? LoginCSS { get; set; }


        [Display(Name = "Layout CSS")]
        public string? LayoutCSS { get; set; }


        [Display(Name = "Layout Javascript")]
        public string? LayoutJavaScript { get; set; }

        [Display(Name = "Dashboard CSS")]
        public string? DashboardCSS { get; set; }

        [Display(Name = "Redirect To https//www.")]
        public bool RedirectTohttpswww { get; set; }

        [Display(Name = "Redirect To https//")]
        public bool RedirectTohttps { get; set; }

        [Display(Name = "Configuration")]
        public string? Configuration { get; set; }

        [Display(Name = "Live Configuration")]
        public string? LiveConfiguration { get; set; }


        [Display(Name = "Mail Sender")]
        public MailSender MailSender { get; set; }

        [Display(Name = "Outlook Server")]
        public string? OutlookServer { get; set; }
        [Display(Name = "Outlook Port")]
        public int OutlookPort { get; set; }
        [Display(Name = "OutlookUsername")]
        public string? OutlookUsername { get; set; }
        [Display(Name = "Outlook Security")]
        public string? OutlookSecurity { get; set; }
        [Display(Name = "Outlook Sender Email")]
        public string? OutlookSenderEmail { get; set; }
        [Display(Name = "Outlook SSL Enable")]
        public bool OutlookSSLEnable { get; set; }
        [Display(Name = "Outlook Use Default Credentials")]
        public bool OutlookUseDefaultCredentials { get; set; }


        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }

        [Display(Name = "Google Server")]
        public string? GoogleServer { get; set; }
        [Display(Name = "Google Port")]
        public int GooglePort { get; set; }
        [Display(Name = "GoogleUsername")]
        public string? GoogleUsername { get; set; }
        [Display(Name = "Google Security")]
        public string? GoogleSecurity { get; set; }
        [Display(Name = "Google Sender Email")]
        public string? GoogleSenderEmail { get; set; }
        [Display(Name = "Google SSL Enable")]
        public bool GoogleSSLEnable { get; set; }
        [Display(Name = "Google Use Default Credentials")]
        public bool GoogleUseDefaultCredentials { get; set; }


        [Display(Name = "Webmail Server")]
        public string? WebmailServer { get; set; }
        [Display(Name = "Webmail Port")]
        public int WebmailPort { get; set; }
        [Display(Name = "WebmailUsername")]
        public string? WebmailUsername { get; set; }
        [Display(Name = "Webmail Security")]
        public string? WebmailSecurity { get; set; }
        [Display(Name = "Webmail Sender Email")]
        public string? WebmailSenderEmail { get; set; }
        [Display(Name = "Webmail SSL Enable")]
        public bool WebmailSSLEnable { get; set; }
        [Display(Name = "Webmail Use Default Credentials")]
        public bool WebmailUseDefaultCredentials { get; set; }

        [Display(Name = "CC mails seperate with comma ,")]
        public string? CCmails { get; set; }
        [Display(Name = "BB mails seperate with comma ,")]
        public string? BBmails { get; set; }
    }
}
