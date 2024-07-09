using AgendaManager.Domain.Authorization.ValueObjects;

namespace AgendaManager.TestCommon.TestConstants;

public static partial class Constants
{
    public static class Permissions
    {
        public static readonly PermissionId Id = PermissionId.Create();

        public static readonly string UsersCanDelete = "users:delete";

        public static readonly string UsersCanUpdate = "users:update";

        public static readonly string UsersCanRead = "users:read";

        public static readonly string UsersCanCreate = "users:create";
    }
}
