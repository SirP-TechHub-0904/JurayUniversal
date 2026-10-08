namespace JurayUniversal.MisCentral.Models
{
    public static class CentralRoles
    {
        public const string MasterSuperAdmin = "MasterSuperAdmin";
        public const string CentralAdmin = "CentralAdmin";
        public const string CentralSupport = "CentralSupport";
        public const string CentralAuditor = "CentralAuditor";

        public static readonly string[] AllRoles = new[]
        {
            MasterSuperAdmin,
            CentralAdmin,
            CentralSupport,
            CentralAuditor
        };
    }
}
