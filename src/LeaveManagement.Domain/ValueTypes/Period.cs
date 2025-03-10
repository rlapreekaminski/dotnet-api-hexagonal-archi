using LeaveManagement.Domain.Exceptions;

namespace LeaveManagement.Domain.ValueTypes;

public record Period
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public Period(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new InvalidLeaveRequestException("End date cannot be before start date.");

        if (startDate < DateTime.Today)
            throw new InvalidLeaveRequestException("Start date cannot be in the past.");

        if (IsWeekend(startDate) || IsWeekend(endDate))
            throw new InvalidLeaveRequestException("Start and end dates cannot be on a weekend.");

        StartDate = startDate;
        EndDate = endDate;
    }

    private static bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
}
