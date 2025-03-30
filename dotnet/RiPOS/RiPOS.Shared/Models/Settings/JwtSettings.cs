namespace RiPOS.Shared.Models.Settings
{
    public class JwtSettings
    {
        public required string Issuer { get; set; }

        public required string Audience { get; set; }

        public required string Key { get; set; }
        
        public int AccessTokenExpirationMinutes { get; set; }
        
        public int RefreshTokenExpirationHours { get; set; }
    }
}
