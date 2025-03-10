using LeaveManagement.Application.UseCases;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Ports;
using Moq;
using Xunit;

namespace LeaveManagement.Test.Application
{
    public class SubmitLeaveRequestUseCaseTests
    {
        private readonly Mock<ILeaveRequestRepository> leaveRequestRepositoryMock;
        private readonly SubmitLeaveRequestUseCase submitLeaveRequestUseCase;

        public SubmitLeaveRequestUseCaseTests()
        {
            leaveRequestRepositoryMock = new Mock<ILeaveRequestRepository>();
            submitLeaveRequestUseCase = new SubmitLeaveRequestUseCase(leaveRequestRepositoryMock.Object);
        }

        [Fact]
        public async Task SubmitLeaveRequest_Should_Call_Add_Method_Once_And_Return_Id()
        {
            // Arrange
            leaveRequestRepositoryMock.Setup(x => x.AddAsync(It.IsAny<LeaveRequest>(), It.IsAny<CancellationToken>()))
                                       .Returns(Task.CompletedTask);

            // Act
            var result = await submitLeaveRequestUseCase.Execute("employee_1234567890ABCDEFGHIJKLMNOPQ", new DateTime(2025, 3, 10), new DateTime(2025, 3, 14), LeaveType.Vacation, "comment");

            // Assert
            leaveRequestRepositoryMock.Verify(x => x.AddAsync(It.IsAny<LeaveRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }
    }
}
