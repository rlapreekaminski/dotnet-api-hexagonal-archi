using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Domain.Ports.Driven;

public interface ILeaveRequestRepository
{
    Task AddAsync(LeaveRequest request, CancellationToken ct = default);

    Task UpdateAsync(LeaveRequest request, CancellationToken ct = default);

    Task<LeaveRequest> GetByIdAsync(string id, CancellationToken ct = default);
}
