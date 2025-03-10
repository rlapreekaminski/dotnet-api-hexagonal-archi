using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Ports.Driven;
using LeaveManagement.Domain.Ports.Driving;

namespace LeaveManagement.Application.UseCases;

public class SubmitLeaveRequestUseCase(ILeaveRequestRepository repository) : ISubmitLeaveRequestUseCase
{
    public async Task<LeaveRequest> Execute(string employeeId, DateTime startDate, DateTime endDate, LeaveType type, string? comment, CancellationToken ct = default)
    {
        LeaveRequest leaveRequest = new (employeeId, startDate, endDate, type, comment);
        await repository.AddAsync(leaveRequest, ct);
        return leaveRequest;
    }
}
