namespace Traplin.Core.Entities
{
    public class ModerationQueue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EntityType { get; set; } = string.Empty; // company, opportunity
        public Guid EntityId { get; set; }
        public Guid ModeratorId { get; set; }
        public string Action { get; set; } = string.Empty; // create, update
        public string Status { get; set; } = "pending"; // pending, approved, rejected
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual AppUser Moderator { get; set; } = null!;
    }
}