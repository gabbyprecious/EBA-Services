using System;
namespace FinquestTest.Models
{
    public class ListResponseUser
    {
        public ListResponseUser(string id, string firstName, string lastName, DateTime? last)
        {
            this.DisplayName = $"{firstName} {lastName}";
            this.FirstName = firstName;
            this.LastName = lastName;
            this.LastConnectionDate = last;
            this.Id = id;
        }
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public DateTime? LastConnectionDate { get; set; }
    }
}

