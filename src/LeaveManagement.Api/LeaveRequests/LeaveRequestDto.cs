namespace LeaveManagement.Api.LeaveRequests;

public record LeaveRequestDto(string Id, string EmployeeId, DateTime StartDate, DateTime EndDate, string Type, string Status, string? Comment, string? ManagerComment);
