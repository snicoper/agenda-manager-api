﻿using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.UpdateResource;

[Authorize(Permissions = SystemPermissions.Resources.Update)]
public record UpdateResourceCommand(
    Guid ResourceId,
    string Name,
    string Description,
    string TextColor,
    string BackgroundColor) : ICommand;
