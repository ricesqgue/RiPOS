namespace RiPOS.Shared.Models.Responses;

public class TokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}