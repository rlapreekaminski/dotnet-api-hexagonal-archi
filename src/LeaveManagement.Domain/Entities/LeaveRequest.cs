using KSUID;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Domain.ValueTypes;

namespace LeaveManagement.Domain.Entities;

public class LeaveRequest(string employeeId, DateTime startDate, DateTime endDate, LeaveType type, string? comment)
{
    public string Id { get; private set; } = $"leave-request_{Ksuid.Generate()}";
    public string EmployeeId { get; private set; } = employeeId;
    public Period Period { get; private set; } = new Period(startDate, endDate);
    public LeaveType Type { get; private set; } = type;
    public LeaveStatus Status { get; private set; } = LeaveStatus.Pending;
    public string? Comment { get; private set; } = comment;
    public string? ManagerComment { get; private set; }

    public static LeaveRequest Rehydrate(string id, string employeeId, DateTime startDate, DateTime endDate, string type, string status, string? comment, string? managerComment)
    {
        return new LeaveRequest(employeeId, startDate, endDate, Enum.Parse<LeaveType>(type), comment)
        {
            Id = id,
            Status = Enum.Parse<LeaveStatus>(status),
            ManagerComment = managerComment
        };
    }

    public void ChangeStatus(LeaveStatus status, string? managerComment)
    {
        if (Status != LeaveStatus.Pending)
        {
            throw new InvalidLeaveRequestException($"Only {nameof(LeaveStatus.Pending)} leave requests can be updated.");
        }

        Status = status;
        ManagerComment = managerComment;
    }
}