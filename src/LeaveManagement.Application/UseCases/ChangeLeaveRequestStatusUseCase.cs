using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Ports.Driven;
using LeaveManagement.Domain.Ports.Driving;

namespace LeaveManagement.Application.UseCases;

public class ChangeLeaveRequestStatusUseCase(ILeaveRequestRepository repository) : IChangeLeaveRequestStatusUseCase
{
    public async Task<LeaveRequest> Execute(string leaveRequestId, LeaveStatus status, string? managerComment, CancellationToken ct = default)
    {
        LeaveRequest leaveRequest = await repository.GetByIdAsync(leaveRequestId, ct);
        leaveRequest.ChangeStatus(status, managerComment);
        await repository.UpdateAsync(leaveRequest, ct);
        return leaveRequest;
    }
}
