namespace LeaveManagement.Domain.Exceptions;

public class RepositoryException : Exception
{
    public RepositoryException(string message) : base(message) { }

    public RepositoryException(Exception e, string message) : base(message, e) { }
}
