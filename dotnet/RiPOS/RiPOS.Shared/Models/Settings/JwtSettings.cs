namespace RiPOS.Shared.Models.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }
        
        public int AccessTokenExpirationMinutes { get; set; }
        
        public int RefreshTokenExpirationHours { get; set; }
    }
}
