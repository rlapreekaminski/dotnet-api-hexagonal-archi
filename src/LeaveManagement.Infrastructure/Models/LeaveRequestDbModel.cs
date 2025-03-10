using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Infrastructure.Models;

public class LeaveRequestDbModel
{
    public string Id { get; private set; } = string.Empty;
    public string EmployeeId { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public string? Comment { get; init; }
    public string? ManagerComment { get; init; }

    public LeaveRequestDbModel() { }

    public LeaveRequestDbModel(LeaveRequest leaveRequest)
    {
        Id = leaveRequest.Id;
        EmployeeId = leaveRequest.EmployeeId;
        StartDate = leaveRequest.Period.StartDate;
        EndDate = leaveRequest.Period.EndDate;
        Status = leaveRequest.Status.ToString();
        Type = leaveRequest.Type.ToString();
        Comment = leaveRequest.Comment;
        ManagerComment = leaveRequest.ManagerComment;
    }

    public LeaveRequest ToDomain()
    {
        return LeaveRequest.Rehydrate(
            Id,
            EmployeeId,
            StartDate,
            EndDate,
            Type,
            Status,
            Comment,
            ManagerComment
        );
    }

}
