namespace RiPOS.Shared.Models.Responses
{
    public class CashRegisterResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool IsActive { get; set; }
        
        public int StoreId { get; set; }
    }
}
