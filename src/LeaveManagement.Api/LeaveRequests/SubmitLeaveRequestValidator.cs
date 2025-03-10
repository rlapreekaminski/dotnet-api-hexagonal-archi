using FluentValidation;
using LeaveManagement.Domain.Enums;
using System.Text.RegularExpressions;

namespace LeaveManagement.Api.LeaveRequests;

public partial class SubmitLeaveRequestValidator : AbstractValidator<SubmitLeaveRequestDto>
{

    [GeneratedRegex(@"^employee_[a-zA-Z0-9]{27}$")]
    private static partial Regex EmployeeIdRegex();

    public SubmitLeaveRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage($"{nameof(SubmitLeaveRequestDto.EmployeeId)} is required.")
            .Matches(EmployeeIdRegex()).WithMessage($"{nameof(SubmitLeaveRequestDto.EmployeeId)} must start with 'employee_' followed by 27 alphanumeric characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage($"{nameof(SubmitLeaveRequestDto.StartDate)} is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage($"{nameof(SubmitLeaveRequestDto.EndDate)} is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage($"{nameof(SubmitLeaveRequestDto.Type)} is required.")
            .IsEnumName(typeof(LeaveType), caseSensitive: false).WithMessage($"Invalid {nameof(SubmitLeaveRequestDto.Type)}. Allowed values: {string.Join(", ",Enum.GetValues<LeaveType>())}.");
    }
}
