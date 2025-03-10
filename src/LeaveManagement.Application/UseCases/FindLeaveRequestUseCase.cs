using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Ports.Driven;
using LeaveManagement.Domain.Ports.Driving;

namespace LeaveManagement.Application.UseCases;

public class FindLeaveRequestUseCase(ILeaveRequestRepository repository) : IFindLeaveRequestUseCase
{
    public Task<LeaveRequest> Execute(string leaveRequestId, CancellationToken ct = default) => repository.GetByIdAsync(leaveRequestId, ct);
}
