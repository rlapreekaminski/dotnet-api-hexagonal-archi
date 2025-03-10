using FluentAssertions;
using LeaveManagement.Application.UseCases;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Exceptions;
using LeaveManagement.Infrastructure.Models;
using LeaveManagement.Infrastructure.Repositories;
using Moq;

namespace LeaveManagement.Test.Application;

public class ChangeLeaveRequestStatusUseCaseTests
{
    private readonly Mock<ISafeDapperWrapper> dapperWrapper;
    private readonly LeaveRequestRepository leaveRequestRepository;
    private readonly ChangeLeaveRequestStatusUseCase changeLeaveRequestStatusUseCase;

    public ChangeLeaveRequestStatusUseCaseTests()
    {
        dapperWrapper = new Mock<ISafeDapperWrapper>();
        leaveRequestRepository = new LeaveRequestRepository(dapperWrapper.Object);
        changeLeaveRequestStatusUseCase = new ChangeLeaveRequestStatusUseCase(leaveRequestRepository);
    }

    [Fact]
    public async Task ChangingLeaveRequestStatus_Should_Success_If_Approved_An_Existing_Pending_LeaveRequest()
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

        var sqlUpdate = @"
            UPDATE LeaveRequests
            SET EmployeeId = @EmployeeId,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Type = @Type,
                Comment = @Comment,
                Status = @Status,
                ManagerComment = @ManagerComment
            WHERE Id = @Id";

        // Act
        LeaveRequest updatedLeaveRequest = await changeLeaveRequestStatusUseCase.Execute("leave-request_1234567890ABCDEFGHIJKLMNOPQ", LeaveStatus.Approved, "comment");

        // Assert
        dapperWrapper.Verify(x => x.SafeExecuteAsync(sqlUpdate, It.IsAny<LeaveRequestDbModel>(), It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Once);
        updatedLeaveRequest.Should().NotBeNull();
        updatedLeaveRequest.Status.Should().Be(LeaveStatus.Approved);
    }

    [Fact]
    public async Task ChangingLeaveRequestStatus_Should_Success_If_Rejected_An_Existing_Pending_LeaveRequest()
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

        var sqlUpdate = @"
            UPDATE LeaveRequests
            SET EmployeeId = @EmployeeId,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Type = @Type,
                Comment = @Comment,
                Status = @Status,
                ManagerComment = @ManagerComment
            WHERE Id = @Id";
        dapperWrapper.Setup(x => x.SafeExecuteAsync(sqlUpdate, It.IsAny<LeaveRequestDbModel>(), It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .ReturnsAsync(1);

        // Act
        LeaveRequest updatedLeaveRequest = await changeLeaveRequestStatusUseCase.Execute("leave-request_1234567890ABCDEFGHIJKLMNOPQ", LeaveStatus.Rejected, "comment");

        // Assert
        updatedLeaveRequest.Should().NotBeNull();
        updatedLeaveRequest.Status.Should().Be(LeaveStatus.Rejected);
    }

    [Fact]
    public async Task ChangingLeaveRequestStatus_Should_Failed_If_Rejected_An_Existing_Approved_LeaveRequest()
    {
        // Arrange
        LeaveRequest leaveRequest = LeaveRequest.Rehydrate(
            "leave-request_1234567890ABCDEFGHIJKLMNOPQ",
            "employee_1234567890ABCDEFGHIJKLMNOPQ",
            new DateTime(2025, 3, 10),
            new DateTime(2025, 3, 14),
            "Vacation",
            "Approved",
            "comment",
            null
        );
        string sql = @"
            SELECT * FROM LeaveRequests 
            WHERE Id = @Id";
        dapperWrapper.Setup(x => x.SafeReadAsync<LeaveRequestDbModel>(sql, "leave-request_1234567890ABCDEFGHIJKLMNOPQ", It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .ReturnsAsync(new LeaveRequestDbModel(leaveRequest));

        // Act
        Func<Task> act = async () => await changeLeaveRequestStatusUseCase.Execute("leave-request_1234567890ABCDEFGHIJKLMNOPQ", LeaveStatus.Rejected, "comment");

        // Assert
        await act.Should().ThrowAsync<InvalidLeaveRequestException>().WithMessage("Only Pending leave requests can be updated.");
    }

    [Fact]
    public async Task ChangingLeaveRequestStatus_Should_Failed_If_LeaveRequest_Not_Exists()
    {
        // Arrange
        string sql = @"
            SELECT * FROM LeaveRequests 
            WHERE Id = @Id";

        dapperWrapper.Setup(x => x.SafeReadAsync<LeaveRequestDbModel?>(sql, "leave-request_1234567890ABCDEFGHIJKLMNOPQ", It.IsAny<CancellationToken>(), It.IsAny<string>()))
            .ThrowsAsync(new RepositoryNotFoundException("No entity found with leave-request_1234567890ABCDEFGHIJKLMNOPQ"));

        // Act
        Func<Task> act = async () => await changeLeaveRequestStatusUseCase.Execute("leave-request_1234567890ABCDEFGHIJKLMNOPQ", LeaveStatus.Approved, "comment");

        // Assert
        dapperWrapper.Verify(x => x.SafeExecuteAsync(It.IsAny<string>(), It.IsAny<LeaveRequestDbModel>(), It.IsAny<CancellationToken>(), It.IsAny<string>()), Times.Never);
        await act.Should().ThrowAsync<RepositoryNotFoundException>().WithMessage("No entity found with leave-request_1234567890ABCDEFGHIJKLMNOPQ");
    }

}
