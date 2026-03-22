namespace Traplin.Core.Entities
{
    public class Opportunity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CompanyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // internship, vacancy, mentorship, event
        public string WorkFormat { get; set; } = string.Empty; // office, hybrid, remote
        public string Location { get; set; } = string.Empty; // адрес или город
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public DateTime PublicationDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }
        public string? ContactInfo { get; set; }
        public string Status { get; set; } = "pending"; // pending, active, closed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Геоданные для карты
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Навигация
        public virtual Company Company { get; set; } = null!;
        public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
        public virtual ICollection<OpportunityTag> OpportunityTags { get; set; } = new List<OpportunityTag>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}