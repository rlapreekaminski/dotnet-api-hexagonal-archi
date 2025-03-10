using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Domain.Ports.Driving;

public interface IFindLeaveRequestUseCase
{
    Task<LeaveRequest> Execute(string leaveRequestId, CancellationToken ct = default);
}
