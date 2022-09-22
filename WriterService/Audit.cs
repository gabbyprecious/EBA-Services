using System;

namespace WriterService
{
    public class Audit
    {
        public Audit(string createUsername)
        {
            CreationUsername = createUsername;
            CreationDate = DateTime.Now;
        }

        public string CreationUsername { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedUsername { get; set; } = null!;
    }
}

