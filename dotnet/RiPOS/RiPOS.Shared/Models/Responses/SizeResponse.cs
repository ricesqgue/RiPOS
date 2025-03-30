namespace RiPOS.Shared.Models.Responses
{
    public class SizeResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string ShortName { get; set; }

        public bool IsActive { get; set; }
    }
}
