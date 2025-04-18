namespace RiPOS.Shared.Models.Responses;

public class SimpleResponse
{
    public string Message { get; set; } = string.Empty;

    public SimpleResponse()
    {

    }

    public SimpleResponse(string? message)
    {
        Message = message ?? string.Empty;
    }
}