# 🚀 .NET 9 API with Hexagonal architecture

This project is a **minimal .NET API** with an hexagonal architecture around leave request with all OpenAPI configuration.

## 🚧 Context
It implements two User Stories:
### Submit a Leave Request
#### Description:
As an employee, I want to submit a leave request through the HR application to plan my vacations or other absences.

####  Acceptance Criteria:
-To request leave, the employee must enter the start and end dates, the type of leave (vacation, sick leave, etc.), and an optional comment field.
-The form must validate that the start and end dates are correct : the end date cannot be earlier than the start date, dates cannot be in past, start date and end date cannot be Saturday or Sunday

### Approve or Reject a Leave Request
#### Description:
As an HR manager, I want to approve or reject leave requests to efficiently manage employee absences.

#### Acceptance Criteria:
-The manager must be able to approve or reject the request with the option to add an explanatory comment.
-The request status must be updated in real-time in the system and be visible to the employee.

Note: It could be beneficial to further refine the business rules related to leave requests, such as validating that previously submitted leaves do not overlap with a new request, setting a maximum number of days per request and per year, etc.

## Architecture

This project is built in .NET 9 using a hexagonal architecture with four layers: API, Application, Domain, and Infrastructure.

In the Domain layer, there are four ports represented by interfaces:

- Three driving ports (one per Use Case) consumed by the API.
- One driven port, the repository, used for data storage.

A Use Case interacts with the Domain to:

- Submit a new leave request.
- Retrieve a leave request.
- Approve or reject a leave request.

For data storage, I used an SQLite InMemory database, which means a new database is created each time the application starts.

The documentation is generated using OpenAPI and can be accessed via Scalar at https://localhost:7169/scalar.

## 🚧 Improvements

To go further, an identification layer should be added (using Azure Entra ID, for example) to both secure the endpoints (preventing an employee from approving/rejecting requests) and retrieve the employeeId from the JWT token.

Furthermore, the tests in the repository are unit tests that focus on Use Cases and business logic. Therefore, integration tests should be added to verify request handling by the API layer (response codes, intrinsic validation of input data, validation of returned data, etc.).
