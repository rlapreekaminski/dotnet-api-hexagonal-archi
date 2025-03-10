using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Domain.Ports.Driving;

public interface IChangeLeaveRequestStatusUseCase
{
    Task<LeaveRequest> Execute(string leaveRequestId, LeaveStatus status, string? managerComment, CancellationToken ct = default);
}