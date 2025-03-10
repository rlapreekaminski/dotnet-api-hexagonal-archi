using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Ports.Driving;
using System.ComponentModel;

namespace LeaveManagement.Api.LeaveRequests;

public static class LeaveRequestQueryEndpoints
{
    public static void MapLeaveRequestQueryEndpoints(RouteGroupBuilder group)
    {
        group.MapGet("/{leaveRequestId}", async (
            [Description("Leaver request unique identifier")] string leaveRequestId,
            IFindLeaveRequestUseCase useCase,
            CancellationToken ct = default) =>
        {
            LeaveRequest leaveRequest = await useCase.Execute(leaveRequestId, ct);

            LeaveRequestDto leaveRequestDto = new(
                leaveRequest.Id,
                leaveRequest.EmployeeId,
                leaveRequest.Period.StartDate,
                leaveRequest.Period.EndDate,
                leaveRequest.Type.ToString(),
                leaveRequest.Status.ToString(),
                leaveRequest.Comment,
                leaveRequest.ManagerComment);

            return TypedResults.Ok(leaveRequestDto);
        })
        .Produces<LeaveRequestDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithSummary("Retrieve a leave request by its id")
        .WithName("GetLeaveRequestById")
        .WithOpenApi();
    }
}
