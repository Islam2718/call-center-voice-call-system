using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CallCenterPlatform.Application.Features.Clients.Commands;
using CallCenterPlatform.Application.Features.Clients.Queries;
using CallCenterPlatform.Application.DTOs;

namespace CallCenterPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // All endpoints require authentication
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/clients
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
    {
        var clients = await _mediator.Send(new GetAllClientsQuery(includeInactive));
        return Ok(clients);
    }

    // GET: api/clients/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var client = await _mediator.Send(new GetClientByIdQuery(id));
        return Ok(client);
    }

    // POST: api/clients
    [HttpPost]
    [Authorize(Roles = "Admin,Supervisor")]  // Only Admin/Supervisor can create
    public async Task<IActionResult> Create([FromBody] CreateClientRequestDto request)
    {
        var result = await _mediator.Send(new CreateClientCommand(request));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT: api/clients/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClientRequestDto request)
    {
        var result = await _mediator.Send(new UpdateClientCommand(id, request));
        return Ok(result);
    }

    // DELETE: api/clients/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]  // Only Admin can hard delete
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteClientCommand(id));
        return Ok(new { Success = result, Message = "Client deleted successfully" });
    }

    // PATCH: api/clients/{id}/soft-delete
    [HttpPatch("{id}/soft-delete")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var result = await _mediator.Send(new SoftDeleteClientCommand(id));
        return Ok(new { Success = result, Message = "Client deactivated successfully" });
    }
}