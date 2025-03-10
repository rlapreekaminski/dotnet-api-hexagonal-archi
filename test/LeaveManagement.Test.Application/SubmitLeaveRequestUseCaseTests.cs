using FluentAssertions;
using LeaveManagement.Application.UseCases;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Infrastructure.Models;
using LeaveManagement.Infrastructure.Repositories;
using Moq;

namespace LeaveManagement.Test.Application;

public class SubmitLeaveRequestUseCaseTests
{
    private readonly Mock<ISafeDapperWrapper> dapperWrapper;
    private readonly LeaveRequestRepository leaveRequestRepository;
    private readonly SubmitLeaveRequestUseCase submitLeaveRequestUseCase;

    private static DateTime NextMonday => DateTime.Now.AddDays(((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek + 7) % 7);

    public SubmitLeaveRequestUseCaseTests()
    {
        dapperWrapper = new Mock<ISafeDapperWrapper>();
        leaveRequestRepository = new LeaveRequestRepository(dapperWrapper.Object);
        submitLeaveRequestUseCase = new SubmitLeaveRequestUseCase(leaveRequestRepository);
    }

    [Fact]
    public async Task SubmitLeaveRequest_Should_Success_If_Request_Is_Valid()
    {
        // Arrange
        string sql = "INSERT INTO LeaveRequests (Id, EmployeeId, StartDate, EndDate, Type, Status, Comment) VALUES (@Id, @EmployeeId, @StartDate, @EndDate, @Type, @Status, @Comment)";

        dapperWrapper.Setup(x => x.SafeExecuteAsync(sql, It.IsAny<LeaveRequestDbModel>(), It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .ReturnsAsync(1);

        // Act
        LeaveRequest leaveRequest = await submitLeaveRequestUseCase.Execute("employee_1234567890ABCDEFGHIJKLMNOPQ", NextMonday, GetNextDayOfWeekFromDate(DayOfWeek.Friday, NextMonday), LeaveType.Vacation, "comment");

        // Assert
        leaveRequest.Should().NotBeNull();
        leaveRequest.Id.Should().MatchRegex(@"^leave-request_[a-zA-Z0-9]{27}$");
    }

    [Fact]
    public async Task SubmitLeaveRequest_Should_Failed_If_EndDate_Is_Before_StartDate()
    {
        // Act
        Func<Task> act = async () => await submitLeaveRequestUseCase.Execute("employee_1234567890ABCDEFGHIJKLMNOPQ", GetNextDayOfWeekFromDate(DayOfWeek.Friday, NextMonday), NextMonday, LeaveType.Vacation, "comment");
        // Assert
        await act.Should().ThrowAsync<InvalidLeaveRequestException>().WithMessage("End date cannot be before start date.");
    }

    [Fact]
    public async Task SubmitLeaveRequest_Should_Failed_If_StartDate_Is_Passed()
    {
        // Act
        Func<Task> act = async () => await submitLeaveRequestUseCase.Execute("employee_1234567890ABCDEFGHIJKLMNOPQ", new DateTime(2022, 3, 10), NextMonday, LeaveType.Vacation, "comment");

        // Assert
        await act.Should().ThrowAsync<InvalidLeaveRequestException>().WithMessage("Start date cannot be in the past.");
    }

    [Fact]
    public async Task SubmitLeaveRequest_Should_Failed_If_StartDate_Is_WeekEndDay()
    {
        // Act
        Func<Task> act = async () => await submitLeaveRequestUseCase.Execute("employee_1234567890ABCDEFGHIJKLMNOPQ", GetNextDayOfWeekFromDate(DayOfWeek.Saturday, NextMonday), NextMonday.AddDays(7), LeaveType.Vacation, "comment");
        // Assert
        await act.Should().ThrowAsync<InvalidLeaveRequestException>().WithMessage("Start and end dates cannot be on a weekend.");
    }

    [Fact]
    public async Task SubmitLeaveRequest_Should_Failed_If_EndDate_Is_WeekEndDay()
    {
        // Act
        Func<Task> act = async () => await submitLeaveRequestUseCase.Execute("employee_1234567890ABCDEFGHIJKLMNOPQ", NextMonday, GetNextDayOfWeekFromDate(DayOfWeek.Saturday, NextMonday), LeaveType.Vacation, "comment");
        // Assert
        await act.Should().ThrowAsync<InvalidLeaveRequestException>().WithMessage("Start and end dates cannot be on a weekend.");
    }

    private static DateTime GetNextDayOfWeekFromDate(DayOfWeek dayOfWeek, DateTime date) => date.AddDays(((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7);
}
