using System;
namespace WriterService
{
    public class User
    {
        public User(string id, string firstName, string lastName, string username)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Username = username;
            this.Id = id;
        }
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;
    }
}

