using System.Runtime.CompilerServices;

namespace LeaveManagement.Infrastructure.Repositories;

public interface ISafeDapperWrapper
{
    Task<T> SafeReadAsync<T>(string sql, string id, CancellationToken ct = default, [CallerMemberName] string caller = "");
    Task<int> SafeExecuteAsync(string sql, object param, CancellationToken ct = default, [CallerMemberName] string caller = "");
}
