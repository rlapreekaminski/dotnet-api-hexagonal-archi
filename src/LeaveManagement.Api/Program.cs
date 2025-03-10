using Asp.Versioning;
using FluentValidation;
using LeaveManagement.Api.Exceptions;
using LeaveManagement.Api.LeaveRequests;
using LeaveManagement.Application.UseCases;
using LeaveManagement.Domain.Ports.Driven;
using LeaveManagement.Domain.Ports.Driving;
using LeaveManagement.Infrastructure.Configuration;
using LeaveManagement.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(DbConfiguration.CreateDbConnection());
builder.Services.AddSingleton<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddSingleton<ISafeDapperWrapper, SafeDapperWrapper>();

builder.Services.AddTransient<ISubmitLeaveRequestUseCase, SubmitLeaveRequestUseCase>();
builder.Services.AddTransient<IFindLeaveRequestUseCase, FindLeaveRequestUseCase>();
builder.Services.AddTransient<IChangeLeaveRequestStatusUseCase, ChangeLeaveRequestStatusUseCase>();

builder.Services.AddValidatorsFromAssemblyContaining<SubmitLeaveRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateLeaveRequestStatusValidator>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = ApiVersion.Default;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Api-Version")
    );
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.ConfigureExceptionHandler();

app.MapLeaveRequestEndpoints();

app.UseHttpsRedirection();

app.Run();

