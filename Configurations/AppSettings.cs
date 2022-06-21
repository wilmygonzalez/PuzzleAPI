using System;
namespace PuzzleAPI.Configurations
{
    public class AppSettings
    {
        public string DefaultAdminFirstName { get; set; }
        public string DefaultAdminLastName { get; set; }
        public string DefaultAdminEmail { get; set; }
        public string DefaultAdminPassword { get; set; }
        public string Secret { get; set; }
        public int TokenExpiresInDays { get; set; }
    }
}

