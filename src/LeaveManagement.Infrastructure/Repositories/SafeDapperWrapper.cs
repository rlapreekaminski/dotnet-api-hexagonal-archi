using Dapper;
using LeaveManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace LeaveManagement.Infrastructure.Repositories;

public class SafeDapperWrapper(ILogger<SafeDapperWrapper> logger, IDbConnection dbConnection) : ISafeDapperWrapper
{
    public async Task<T> SafeReadAsync<T>(string sql, string id, CancellationToken ct = default, [CallerMemberName] string caller = "")
    {
        try
        {
            T? entity = await dbConnection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return entity ?? throw new RepositoryNotFoundException($"No entity found with {id}");
        }
        catch (RepositoryNotFoundException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to read entity in {caller} with {id}", caller, id);
            throw new RepositoryException(e, "Failed to execute request");
        }
    }

    public async Task<int> SafeExecuteAsync(string sql, object param, CancellationToken ct = default, [CallerMemberName] string caller = "")
    {
        try
        {
            int rowCreated = await dbConnection.ExecuteAsync(new CommandDefinition(sql, param, cancellationToken: ct));
            return rowCreated != 0 ? rowCreated : throw new RepositoryException("Failed to execute request");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to execute request {caller} with {param}", caller, JsonSerializer.Serialize(param));
            throw new RepositoryException(e, "Failed to execute request");
        }
    }
}
