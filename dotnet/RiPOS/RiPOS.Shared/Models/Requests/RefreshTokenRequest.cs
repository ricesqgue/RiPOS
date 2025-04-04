using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests;

public class RefreshTokenRequest
{
    [Required]
    public required string AccessToken { get; set; }
}