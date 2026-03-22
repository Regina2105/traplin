namespace Traplin.Core.Entities
{
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // владелец (работодатель)
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Industry { get; set; }
        public string? Website { get; set; }
        public string? SocialLinks { get; set; } // JSON
        public string VerificationStatus { get; set; } = "pending"; // pending, verified, rejected
        public string? VerificationMethod { get; set; } // email, inn
        public DateTime? VerifiedAt { get; set; }

        // Навигация
        public virtual AppUser User { get; set; } = null!;
        public virtual ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    }
}