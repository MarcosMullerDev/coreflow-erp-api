namespace CoreFlow.Application.Leads.DTOs;

public class CreateLeadNoteRequest
{
    public string Note { get; set; } = string.Empty;
    public DateTime? FollowUpAt { get; set; }
}
