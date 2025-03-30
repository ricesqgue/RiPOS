namespace RiPOS.Shared.Models.Responses
{
    public class CountryStateResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string ShortName { get; set; }
    }
}
