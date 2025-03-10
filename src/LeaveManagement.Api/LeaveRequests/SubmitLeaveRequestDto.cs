using System.ComponentModel;

namespace LeaveManagement.Api.LeaveRequests;

public record SubmitLeaveRequestDto
{
    [property: Description("The unique employee identifier")]
    public string? EmployeeId { get; init; }

    [property: Description("The desire start date for the leave request")]
    public DateTime? StartDate { get; init; }

    [property: Description("The desire end date for the leave request")]
    public DateTime? EndDate { get; init; }

    [property: Description("The leave request type - Accepted values are \"Vacation\", \"Sick\", \"Other\"")]
    public string? Type { get; init; }

    [property: Description("(Optionnal) A comment to justify the leave request ")]
    public string? Comment { get; init; }
}
