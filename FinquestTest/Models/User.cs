using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinquestTest.Models
{
    public class User
    {
        public User(string firstName, string lastName, string username, string password, Audit audit)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Username = username;
            this.Password = password;
            this.Audit = audit;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? BirthDate { get; set; }

        public string Password { get; set; } = null!;

        public Audit? Audit { get; set; } = null!;

        public DateTime? LastConnectionDate { get; set; }
    }
}

