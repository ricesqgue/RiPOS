namespace RiPOS.Shared.Models.Responses;

public class TokenResponse: LoginResponse
{
    public required string RefreshToken { get; set; }
}