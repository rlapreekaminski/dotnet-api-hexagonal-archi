using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Ports.Driven;
using LeaveManagement.Infrastructure.Models;

namespace LeaveManagement.Infrastructure.Repositories;

public class LeaveRequestRepository(ISafeDapperWrapper dapperWrapper) : ILeaveRequestRepository
{
    private readonly ISafeDapperWrapper dapperWrapper = dapperWrapper;

    public Task AddAsync(LeaveRequest request, CancellationToken ct = default)
    {
        string sql = @"
            INSERT INTO LeaveRequests (Id, EmployeeId, StartDate, EndDate, Type, Status, Comment)
            VALUES (@Id, @EmployeeId, @StartDate, @EndDate, @Type, @Status, @Comment)";
        LeaveRequestDbModel param = new(request);
        return dapperWrapper.SafeExecuteAsync(sql, param, ct);
    }

    public async Task<LeaveRequest> GetByIdAsync(string id, CancellationToken ct = default)
    {
        string sql = @"
            SELECT * FROM LeaveRequests 
            WHERE Id = @Id";
        LeaveRequestDbModel leaveRequestDb = await dapperWrapper.SafeReadAsync<LeaveRequestDbModel>(sql, id, ct);
        return leaveRequestDb.ToDomain();
    }

    public Task UpdateAsync(LeaveRequest request, CancellationToken ct = default)
    {
        var sql = @"
            UPDATE LeaveRequests
            SET EmployeeId = @EmployeeId,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Type = @Type,
                Comment = @Comment,
                Status = @Status,
                ManagerComment = @ManagerComment
            WHERE Id = @Id";
        LeaveRequestDbModel param = new(request);
        return dapperWrapper.SafeExecuteAsync(sql, param, ct);
    }
}