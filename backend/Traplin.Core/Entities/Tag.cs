namespace Traplin.Core.Entities
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // tech, level, employment
        public bool IsSystem { get; set; } = true;

        public virtual ICollection<OpportunityTag> OpportunityTags { get; set; } = new List<OpportunityTag>();
    }
}