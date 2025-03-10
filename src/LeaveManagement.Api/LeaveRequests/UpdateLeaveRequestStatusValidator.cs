using FluentValidation;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Api.LeaveRequests;

public class UpdateLeaveRequestStatusValidator : AbstractValidator<UpdateLeaveRequestStatusDto>
{
    public UpdateLeaveRequestStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage($"{nameof(UpdateLeaveRequestStatusDto.Status)} is required.")
            .IsEnumName(typeof(LeaveStatus), caseSensitive: false)
                .WithMessage($"Invalid {nameof(UpdateLeaveRequestStatusDto.Status)}. Allowed values: {string.Join(", ", Enum.GetValues<LeaveStatus>().Where(s => s != LeaveStatus.Pending))}.");
    }
}
