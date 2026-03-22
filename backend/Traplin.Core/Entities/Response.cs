namespace Traplin.Core.Entities
{
    public class Response
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid OpportunityId { get; set; }
        public string Status { get; set; } = "pending"; // pending, accepted, rejected, reserve
        public string? CoverLetter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual AppUser User { get; set; } = null!;
        public virtual Opportunity Opportunity { get; set; } = null!;
    }
}