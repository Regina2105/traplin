using Microsoft.AspNetCore.Identity;

namespace Traplin.Core.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? University { get; set; }
        public int? GraduationYear { get; set; }
        public string? Resume { get; set; }          // текст резюме или ссылка
        public string? PortfolioLinks { get; set; }   // JSON-строка или разделённые ссылки
        public string PrivacyLevel { get; set; } = "public"; // public, friends, private
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public virtual Company? Company { get; set; }
        public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<Friend> FriendsInitiated { get; set; } = new List<Friend>();
        public virtual ICollection<Friend> FriendsReceived { get; set; } = new List<Friend>();
        public virtual ICollection<Opportunity> CreatedOpportunities { get; set; } = new List<Opportunity>(); // для работодателя
        public virtual ICollection<ModerationQueue> Moderations { get; set; } = new List<ModerationQueue>();
    }
}