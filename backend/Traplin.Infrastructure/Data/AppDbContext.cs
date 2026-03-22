using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Traplin.Core.Entities;

namespace Traplin.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<OpportunityTag> OpportunityTags { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ModerationQueue> ModerationQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. User ↔ Company (один-к-одному)
            builder.Entity<Company>()
                .HasOne(c => c.User)
                .WithOne(u => u.Company)
                .HasForeignKey<Company>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Company → Opportunities (один-ко-многим)
            builder.Entity<Opportunity>()
                .HasOne(o => o.Company)
                .WithMany(c => c.Opportunities)
                .HasForeignKey(o => o.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. User → Responses
            builder.Entity<Response>()
                .HasOne(r => r.User)
                .WithMany(u => u.Responses)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. Opportunity → Responses
            builder.Entity<Response>()
                .HasOne(r => r.Opportunity)
                .WithMany(o => o.Responses)
                .HasForeignKey(r => r.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            // 5. User → Favorites
            builder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 6. Opportunity → Favorites
            builder.Entity<Favorite>()
                .HasOne(f => f.Opportunity)
                .WithMany(o => o.Favorites)
                .HasForeignKey(f => f.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            // 7. Friendship (User и FriendUser ссылаются на AppUser)
            builder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany(u => u.FriendsInitiated)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany(u => u.FriendsReceived)
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            // 8. OpportunityTag – составной первичный ключ
            builder.Entity<OpportunityTag>()
                .HasKey(ot => new { ot.OpportunityId, ot.TagId });

            builder.Entity<OpportunityTag>()
                .HasOne(ot => ot.Opportunity)
                .WithMany(o => o.OpportunityTags)
                .HasForeignKey(ot => ot.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OpportunityTag>()
                .HasOne(ot => ot.Tag)
                .WithMany(t => t.OpportunityTags)
                .HasForeignKey(ot => ot.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            // 9. ModerationQueue
            builder.Entity<ModerationQueue>()
                .HasOne(m => m.Moderator)
                .WithMany(u => u.Moderations)
                .HasForeignKey(m => m.ModeratorId)
                .OnDelete(DeleteBehavior.Restrict);

            // 10. Индексы для ускорения поиска
            builder.Entity<Opportunity>()
                .HasIndex(o => o.Status);
            builder.Entity<Opportunity>()
                .HasIndex(o => o.Type);
            builder.Entity<Opportunity>()
                .HasIndex(o => o.Location);
            builder.Entity<Response>()
                .HasIndex(r => r.Status);
            builder.Entity<Friend>()
                .HasIndex(f => new { f.UserId, f.FriendId }).IsUnique();
            builder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.OpportunityId }).IsUnique();
        }
    }
}