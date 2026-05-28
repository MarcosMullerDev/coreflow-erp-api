using CoreFlow.Domain.Common;

namespace CoreFlow.Domain.Entities;

public class LeadNote : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public Guid LeadId { get; private set; }
    public Lead Lead { get; private set; } = null!;

    public Guid? UserId { get; private set; }
    public User? User { get; private set; }

    public string Note { get; private set; } = string.Empty;
    public DateTime? FollowUpAt { get; private set; }

    private LeadNote() { }

    public LeadNote(
        Guid companyId,
        Guid leadId,
        Guid? userId,
        string note,
        DateTime? followUpAt)
    {
        CompanyId = companyId;
        LeadId = leadId;
        UserId = userId;
        Note = note;
        FollowUpAt = followUpAt;
    }
}
