using FluentValidation;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Ports.Driving;

namespace LeaveManagement.Api.LeaveRequests;

public static class LeaveRequestCommandEndpoints
{
    public static void MapLeaveRequestCommandEndpoints(RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            SubmitLeaveRequestDto dto,
            ISubmitLeaveRequestUseCase useCase,
            IValidator<SubmitLeaveRequestDto> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(dto, cancellationToken);
            if (!validationResult.IsValid)
            {
                return TypedResults.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            LeaveRequest leaveRequest = await useCase.Execute(dto.EmployeeId!, dto.StartDate!.Value, dto.EndDate!.Value, Enum.Parse<LeaveType>(dto.Type!), dto.Comment, cancellationToken);

            LeaveRequestDto leaveRequestDto = new(
                leaveRequest.Id,
                leaveRequest.EmployeeId,
                leaveRequest.Period.StartDate,
                leaveRequest.Period.EndDate,
                leaveRequest.Type.ToString(),
                leaveRequest.Status.ToString(),
                leaveRequest.Comment,
                leaveRequest.ManagerComment);

            return Results.Created($"/api/leave-requests/{leaveRequest.Id}", leaveRequestDto);

        })
        .Produces<LeaveRequestDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithSummary("Submit a leave request")
        .WithName("SubmitLeaveRequest")
        .WithOpenApi();

        group.MapPatch("/{leaveRequestId}", async (
            string leaveRequestId,
            UpdateLeaveRequestStatusDto dto,
            IChangeLeaveRequestStatusUseCase useCase,
            IValidator<UpdateLeaveRequestStatusDto> validator,
            CancellationToken cancellationToken) =>
            {
                var validationResult = await validator.ValidateAsync(dto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return TypedResults.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
                }

                LeaveRequest leaveRequest = await useCase.Execute(leaveRequestId, Enum.Parse<LeaveStatus>(dto.Status!), dto.ManagerComment, cancellationToken);

                LeaveRequestDto leaveRequestDto = new(
                    leaveRequest.Id,
                    leaveRequest.EmployeeId,
                    leaveRequest.Period.StartDate,
                    leaveRequest.Period.EndDate,
                    leaveRequest.Type.ToString(),
                    leaveRequest.Status.ToString(),
                    leaveRequest.Comment,
                    leaveRequest.ManagerComment);

                return Results.Ok(leaveRequestDto);
            })
        .Produces<LeaveRequestDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithSummary("Change some data about a leave request")
        .WithName("PatchLeaveRequest")
        .WithOpenApi();

    }
}
