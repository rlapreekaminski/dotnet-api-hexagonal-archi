using Asp.Versioning;
using Asp.Versioning.Builder;

namespace LeaveManagement.Api.LeaveRequests;

public static class LeaveRequestEndpoints
{
    public static void MapLeaveRequestEndpoints(this IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var apiV1 = app.MapGroup("/api/leave-requests")
            .WithTags("Leave Requests")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);

        LeaveRequestCommandEndpoints.MapLeaveRequestCommandEndpoints(apiV1);
        LeaveRequestQueryEndpoints.MapLeaveRequestQueryEndpoints(apiV1);
    }
}
