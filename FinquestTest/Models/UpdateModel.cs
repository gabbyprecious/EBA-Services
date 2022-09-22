using System;
using MongoDB.Bson.Serialization.Attributes;

namespace FinquestTest.Models
{
    public class UpdateModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Username { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Password { get; set; } = null!;
    }
}

