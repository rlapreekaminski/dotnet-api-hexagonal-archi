namespace LeaveManagement.Domain.Exceptions;

public class InvalidLeaveRequestException(string message) : Exception(message)
{
}
