namespace Traplin.Core.Entities
{
    public class OpportunityTag
    {
        public Guid OpportunityId { get; set; }
        public Guid TagId { get; set; }

        public virtual Opportunity Opportunity { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}