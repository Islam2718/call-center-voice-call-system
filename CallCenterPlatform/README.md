# Call Center Platform

## Purpose

This project is a .NET Core backend prototype for an in-house call-center platform. The target platform will support inbound and outbound calls, agent management, CRM access, call history, and future AI features.

The current implementation is an early foundation. It currently provides authentication and client-management APIs; telephony and call-management features are planned next.

## Goals

- Reduce dependency on an expensive third-party call-center tool.
- Keep customer and call data under company control.
- Provide fast and simple access to customer data for agents.
- Support approximately 50 agents initially and scale toward 500+.
- Leave a clear path for transcription, call summaries, and smart routing later.

## Current Implementation

- .NET Web API with Clean Architecture-style project separation
- JWT authentication and role-based authorization
- BCrypt password hashing
- User registration/login foundation
- Client CRUD and soft-delete support
- EF Core migrations
- Serilog console and rolling-file logging
- OpenAPI/Scalar API documentation

Projects:

- `CallCenterPlatform.Domain` - entities, enums, and interfaces
- `CallCenterPlatform.Application` - commands, queries, validators, DTOs, and use cases
- `CallCenterPlatform.Infrastructure` - EF Core, SQL Server, repositories, JWT services, and migrations
- `CallCenterPlatform.API` - controllers, middleware, configuration, and OpenAPI

## Cost-Effective MVP

Use a modular monolith instead of multiple independently deployed services:

```text
Angular Agent/Admin Portal
            |
       .NET Core API
            |
     SQL Database
            |
 Object storage for recordings
```

MVP scope:

1. User and agent authentication
2. Agent status: Available, Busy, and Offline
3. Simulated inbound call flow for the prototype
4. Outbound call request flow
5. Basic round-robin routing
6. Call records and call events
7. Call notes
8. Paginated call history
9. Basic CRM customer lookup
10. Admin user management

Avoid Kubernetes, Redis, RabbitMQ, multiple microservices, read replicas, and multi-region deployment until actual usage justifies their cost and operational complexity.

## Planned Call Domain

The next core entities should include:

- `Call`: agent, customer, direction, status, start/end time, duration, and recording reference
- `CallEvent`: ringing, accepted, held, resumed, transferred, and ended events
- `AgentStatus`: current state and last activity time
- `CallNote`: notes entered by an agent or supervisor
- `CallRecording`: storage key, duration, retention status, and access metadata

Call lifecycle:

```text
Created -> Ringing -> Accepted -> InProgress -> Completed
                    |             |
                    +-> Missed    +-> Failed
```

## Data Access Rules

Customer and call APIs should support pagination, search, filtering, sorting, `AsNoTracking()` for read-only queries, DTO projection, and indexes on phone, email, status, agent ID, and created date.

Example endpoints:

```text
GET /api/clients?page=1&pageSize=25&search=acme&isActive=true
GET /api/calls?page=1&pageSize=25&agentId={id}&from=2026-01-01&to=2026-01-31
```

Large unfiltered `ToListAsync()` queries should not be used in production. Return `items`, `page`, `pageSize`, and `totalCount` for list endpoints.

## Security Requirements

- Public registration must not allow callers to create Admin or Supervisor accounts.
- Prefer Admin-only user creation or an external identity provider.
- Store JWT keys and database credentials in environment variables, user secrets, or a secrets manager.
- Never commit production secrets to `appsettings.json`.
- Use HTTPS in every non-local environment.
- Apply role and resource-level authorization to protected operations.
- Prefer soft delete where audit history is required.
- Do not expose raw exception messages in production responses.
- Record audit events for login, user changes, call changes, and sensitive data access.

## Future Architecture

When scale or reliability requirements justify it, add a telephony-provider adapter, webhook processing, object storage for recordings, a queue for asynchronous work, Redis for distributed agent presence, separate reporting models, and independent AI processing.

The telephony API should be isolated behind an interface so business logic is not tied directly to one provider.

## AI Readiness

No AI implementation is required for the MVP. Store recordings in object storage using a stable `callId`, publish a `CallCompleted` event, keep metadata structured, store transcription and summary results separately, and run AI processing asynchronously so it cannot block live calls.

## Deployment Strategy

### Environments

- Development: local database and local settings
- Staging: production-like settings for QA and user acceptance testing
- Production: managed database, secure secrets, monitoring, and backups

### CI/CD

1. Restore dependencies.
2. Build the .NET solution.
3. Run unit and integration tests.
4. Build the Angular application when available.
5. Apply reviewed database migrations.
6. Deploy to staging.
7. Require approval before production deployment.

### Backups and rollback

- Use automated database backups and point-in-time restore where supported.
- Keep recordings in durable object storage with a documented retention policy.
- Use health checks and a previous-version rollback strategy.
- Test restore procedures regularly.

## Before Production Checklist

- [ ] Add call, call-event, agent-status, note, and recording models.
- [ ] Integrate a telephony provider through an adapter.
- [ ] Replace LocalDB with a supported shared database.
- [ ] Add pagination, filtering, and search to client and call queries.
- [ ] Restrict user registration and role assignment.
- [ ] Move JWT and database secrets out of configuration files.
- [ ] Add tests for authentication, authorization, routing, and call state transitions.
- [ ] Add health checks, audit logs, monitoring, and alerts.
- [ ] Define recording retention, privacy, and compliance requirements.
- [ ] Verify the solution with the installed .NET SDK before demonstration.

## Prototype Demonstration

Demonstrate one complete flow:

1. Admin creates an agent.
2. Agent logs in.
3. Agent becomes Available.
4. A simulated customer call arrives.
5. The system routes the call to the available agent.
6. The agent accepts and ends the call.
7. The system stores the call and a note.
8. A supervisor views paginated call history.

This proves the core design without requiring production telephony or AI integration.
