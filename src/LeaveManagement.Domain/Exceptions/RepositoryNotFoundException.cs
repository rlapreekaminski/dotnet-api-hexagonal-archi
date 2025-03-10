namespace LeaveManagement.Domain.Exceptions;

public class RepositoryNotFoundException(string message) : Exception(message)
{
}
