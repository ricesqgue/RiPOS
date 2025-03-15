namespace RiPOS.Shared.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string SecondSurname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string MobilePhone { get; set; }

        public bool IsActive { get; set; }

        public ICollection<RoleResponse> Roles { get; set; }
    }
}
