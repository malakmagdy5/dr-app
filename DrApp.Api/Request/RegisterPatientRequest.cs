namespace DrApp.Api.Request
{
    public class RegisterPatientRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int Gender { get; set; }

        public string? BloodType { get; set; }

        public string PasswordHash { get; set; } 
    }
}
