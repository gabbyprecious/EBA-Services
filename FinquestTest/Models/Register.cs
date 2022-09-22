using System;
namespace FinquestTest.Models
{
    public class Register
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public DateTime? BirthDate { get; set; }

        public string Password { get; set; } = null!;
    }

}