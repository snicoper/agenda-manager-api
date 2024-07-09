using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Constants;

namespace AgendaManager.TestCommon.TestConstants;

public static partial class Constants
{
    public static class Role
    {
        public static readonly RoleId Id = RoleId.Create();

        public static readonly string RoleAdmin = Roles.Admin;

        public static readonly string RoleManager = Roles.Manager;

        public static readonly string RoleClient = Roles.Client;
    }
}
