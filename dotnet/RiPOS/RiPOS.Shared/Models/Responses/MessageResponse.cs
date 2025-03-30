namespace RiPOS.Shared.Models.Responses
{
    public class MessageResponse<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}
