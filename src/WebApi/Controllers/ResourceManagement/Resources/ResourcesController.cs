using AgendaManager.Application.Common.Http;
using AgendaManager.Application.ResourceManagement.Resources.ActivateResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.CreateResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.DeactivateResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.DeleteResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.UpdateResource;
using AgendaManager.Application.ResourceManagement.Resources.Queries.GetResourceById;
using AgendaManager.Application.ResourceManagement.Resources.Queries.GetResourcesPaginated;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.ResourceManagement.Resources.Contracts;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.ResourceManagement.Resources;

[Route("api/v{version:apiVersion}/resources")]
public class ResourcesController : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of resources.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetResourcesPaginatedQueryResponse>>>> GetResourcesPaginated(
        [FromQuery] RequestData requestData)
    {
        var query = new GetResourcesPaginatedQuery(requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Get resource by id.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{resourceId:guid}")]
    public async Task<ActionResult<Result<GetResourceByIdQueryResponse>>> GetResourceById(Guid resourceId)
    {
        var query = new GetResourceByIdQuery(resourceId);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new resource.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<Result<CreateResourceCommandResponse>>> CreateResource(CreateResourceRequest request)
    {
        var command = new CreateResourceCommand(
            request.UserId,
            request.ResourceTypeId,
            request.Name,
            request.Description,
            request.TextColor,
            request.BackgroundColor);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Update a resource.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{resourceId:guid}")]
    public async Task<ActionResult<Result>> UpdateResource(Guid resourceId, UpdateResourceRequest request)
    {
        var command = new UpdateResourceCommand(
            resourceId,
            request.Name,
            request.Description,
            request.TextColor,
            request.BackgroundColor);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Desactivar un recurso.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{resourceId:guid}/deactivate")]
    public async Task<ActionResult<Result>> DeactivateResource(Guid resourceId, DeactivateResourceRequest request)
    {
        var command = new DeactivateResourceCommand(resourceId, request.DeactivationReason);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Activar un recurso.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{resourceId:guid}/activate")]
    public async Task<ActionResult<Result>> ActivateResource(Guid resourceId)
    {
        var command = new ActivateResourceCommand(resourceId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Emliminar un recurso.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{resourceId:guid}")]
    public async Task<ActionResult<Result>> DeleteResource(Guid resourceId)
    {
        var command = new DeleteResourceCommand(resourceId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
