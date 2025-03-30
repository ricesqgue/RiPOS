namespace RiPOS.Shared.Models.Responses
{
    public class CustomerResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }

        public string? SecondSurname { get; set; }

        public string? PhoneNumber { get; set; }

        public string? MobilePhone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public string? Rfc { get; set; }

        public required CountryStateResponse CountryState { get; set; }

        public bool IsActive { get; set; }
    }
}
