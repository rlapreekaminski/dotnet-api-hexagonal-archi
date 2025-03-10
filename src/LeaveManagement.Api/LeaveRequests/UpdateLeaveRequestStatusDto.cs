using System.ComponentModel;

namespace LeaveManagement.Api.LeaveRequests;

public record UpdateLeaveRequestStatusDto
{

    [property: Description("The leave request status - Accepted values are \"Approved\", \"Rejected\"")]
    public string? Status { get; init; }

    [property: Description("A comment to justify the status")]
    public string? ManagerComment { get; init; }
}
