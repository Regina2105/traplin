namespace Traplin.Core.Entities
{
    public class Friend
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // инициатор
        public Guid FriendId { get; set; } // получатель
        public string Status { get; set; } = "pending"; // pending, accepted
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual AppUser User { get; set; } = null!;
        public virtual AppUser FriendUser { get; set; } = null!;
    }
}