using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinquestTest.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string BirthDate { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}

