using System;
namespace WriterService
{
    public class User
    {
        public User(string firstName, string lastName, string username, string password)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Username = username;
            this.Password = password;
        }


        public string? Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string Password { get; set; } = null!;

        public Audit? Audit { get; set; } = null!;

        public DateTime? LastConnectionDate { get; set; }
    }
}

