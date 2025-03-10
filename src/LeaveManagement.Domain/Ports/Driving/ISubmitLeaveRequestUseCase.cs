using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Domain.Ports.Driving;

public interface ISubmitLeaveRequestUseCase
{
    Task<LeaveRequest> Execute(string employeeId, DateTime startDate, DateTime endDate, LeaveType type, string? comment, CancellationToken ct = default);
}