using AgendaManager.Application.Common.Http;
using AgendaManager.Application.ResourceManagement.Resources.Commands.ActivateResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.CreateResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.CreateSchedule;
using AgendaManager.Application.ResourceManagement.Resources.Commands.DeactivateResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.DeleteResource;
using AgendaManager.Application.ResourceManagement.Resources.Commands.UpdateResource;
using AgendaManager.Application.ResourceManagement.Resources.Queries.GetResourceById;
using AgendaManager.Application.ResourceManagement.Resources.Queries.GetResourcesPaginated;
using AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceIdPaginated;
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
    /// Get all schedules by resource id.
    /// <para>Get schedules from a CalendarId selected in headers.</para>
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{resourceId:guid}/schedules/paginated")]
    public async Task<ActionResult<Result<ResponseData<GetSchedulesByResourceIdPaginatedQueryResponse>>>>
        GetSchedulesByResourceIdPaginated(Guid resourceId, [FromQuery] RequestData requestData)
    {
        var query = new GetSchedulesByResourceIdPaginatedQuery(resourceId, requestData);
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
            UserId: request.UserId,
            ResourceTypeId: request.ResourceTypeId,
            Name: request.Name,
            Description: request.Description,
            TextColor: request.TextColor,
            BackgroundColor: request.BackgroundColor);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new schedule.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{resourceId:guid}/schedules")]
    public async Task<ActionResult<Result<CreateScheduleCommandResponse>>> CreateSchedule(
        Guid resourceId,
        CreateScheduleRequest request)
    {
        var command = new CreateScheduleCommand(
            resourceId,
            request.Name,
            request.Description,
            request.Type,
            request.AvailableDays,
            request.Start,
            request.End);
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
    /// Eliminar un recurso.
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
