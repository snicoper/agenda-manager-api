﻿using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class RoleFactory
{
    public static Role CreateRoleAdmin(
        RoleId? roleId = null,
        string name = Roles.Admin,
        string description = "Admin role")
    {
        return new Role(roleId ?? RoleId.Create(), name, description);
    }

    public static Role CreateRoleManager(
        RoleId? roleId = null,
        string name = Roles.Manager,
        string description = "Manager role")
    {
        return new Role(roleId ?? RoleId.Create(), name, description);
    }

    public static Role CreateRoleClient(
        RoleId? roleId = null,
        string name = Roles.Client,
        string description = "Client role")
    {
        return new Role(roleId ?? RoleId.Create(), name, description);
    }
}
