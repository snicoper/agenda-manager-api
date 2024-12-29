﻿using AgendaManager.Application.Common.Http;
using AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.CreateResourceType;
using AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.DeleteResourceType;
using AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypesPaginated;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.ResourceManagement.ResourceTypes.Contracts;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.ResourceManagement.ResourceTypes;

[Route("api/v{version:apiVersion}/resource-types")]
public class ResourceTypesController : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of resource types.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetResourceTypesPaginatedQueryResponse>>>>
        GetResourceTypesPaginated([FromQuery] RequestData requestData)
    {
        var query = new GetResourceTypesPaginatedQuery(requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new resource type.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<Result<CreateResourceTypeCommandResponse>>> CreateResourceType(
        CreateResourceTypeRequest request)
    {
        var command = new CreateResourceTypeCommand(
            Name: request.Name,
            Description: request.Description,
            Category: request.Category);

        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a resource type.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{resourceTypeId:guid}")]
    public async Task<ActionResult<Result>> DeleteResourceType(Guid resourceTypeId)
    {
        var command = new DeleteResourceTypeCommand(resourceTypeId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
