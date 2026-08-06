namespace DrApp.Api.Request
{
    public class RegisterDoctorRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int Gender { get; set; }

        public string LiscenceNumber { get; set; }


        public string Degree { get; set; }

        public string Biography { get; set; }
        
        public string PasswordHash { get; set; } 


    }
}
