using Dapper;
using FluentAssertions;
using LeaveManagement.Application.UseCases;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Infrastructure.Models;
using LeaveManagement.Infrastructure.Repositories;
using Moq;

namespace LeaveManagement.Test.Application;

public class FindLeaveRequestUseCaseTests
{
    private readonly Mock<ISafeDapperWrapper> dapperWrapper;
    private readonly LeaveRequestRepository leaveRequestRepository;
    private readonly FindLeaveRequestUseCase findLeaveRequestUseCase;

    public FindLeaveRequestUseCaseTests()
    {
        dapperWrapper = new Mock<ISafeDapperWrapper>();
        leaveRequestRepository = new LeaveRequestRepository(dapperWrapper.Object);
        findLeaveRequestUseCase = new FindLeaveRequestUseCase(leaveRequestRepository);
    }

    [Fact]
    public async Task FindLeaveRequest_Should_Return_Valid_LeaveRequest_If_Id_Exists()
    {
        // Arrange
        LeaveRequest leaveRequest = LeaveRequest.Rehydrate(
            "leave-request_1234567890ABCDEFGHIJKLMNOPQ",
            "employee_1234567890ABCDEFGHIJKLMNOPQ",
            new DateTime(2025, 3, 10),
            new DateTime(2025, 3, 14),
            "Vacation",
            "Pending",
            "comment",
            null
        );

        string sql = @"
            SELECT * FROM LeaveRequests 
            WHERE Id = @Id";
        dapperWrapper.Setup(x => x.SafeReadAsync<LeaveRequestDbModel>(sql, "leave-request_1234567890ABCDEFGHIJKLMNOPQ", It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .ReturnsAsync(new LeaveRequestDbModel(leaveRequest));

        // Act
        LeaveRequest leaveRequestFound = await findLeaveRequestUseCase.Execute("leave-request_1234567890ABCDEFGHIJKLMNOPQ");

        // Assert
        leaveRequestFound.Should().NotBeNull();
        leaveRequestFound.Id.Should().Be("leave-request_1234567890ABCDEFGHIJKLMNOPQ");
        leaveRequestFound.EmployeeId.Should().Be("employee_1234567890ABCDEFGHIJKLMNOPQ");
        leaveRequestFound.Period.StartDate.Should().Be(new DateTime(2025, 3, 10));
        leaveRequestFound.Period.EndDate.Should().Be(new DateTime(2025, 3, 14));
        leaveRequestFound.Type.Should().Be(LeaveType.Vacation);
        leaveRequestFound.Comment.Should().Be("comment");
    }

    [Fact]
    public async Task FindLeaveRequest_Should_Failed_If_Id_Not_Exists()
    {
        // Arrange
        string sql = @"
            SELECT * FROM LeaveRequests 
            WHERE Id = @Id";
        var cmd = new CommandDefinition(sql, new { Id = "leave-request_1234567890ABCDEFGHIJKLMNOPQ" }, cancellationToken: default);

        dapperWrapper.Setup(x => x.SafeReadAsync<LeaveRequestDbModel?>(sql, "leave-request_1234567890ABCDEFGHIJKLMNOPQ", It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .ThrowsAsync(new RepositoryNotFoundException("No entity found with leave-request_1234567890ABCDEFGHIJKLMNOPQ"));

        // Act
        Func<Task> act = async () => await findLeaveRequestUseCase.Execute("leave-request_1234567890ABCDEFGHIJKLMNOPQ");

        // Assert
        await act.Should().ThrowAsync<RepositoryNotFoundException>().WithMessage("No entity found with leave-request_1234567890ABCDEFGHIJKLMNOPQ");
    }
}
