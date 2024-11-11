namespace AgendaManager.Domain.Users.Constants;

public static class SystemPermissions
{
    public static class Appointments
    {
        public const string Create = "appointment:create";
        public const string Read = "appointment:read";
        public const string Update = "appointment:update";
        public const string Delete = "appointment:delete";
    }

    public static class AppointmentStatuses
    {
        public const string Create = "appointment-status:create";
        public const string Read = "appointment-status:read";
        public const string Update = "appointment-status:update";
        public const string Delete = "appointment-status:delete";
    }

    public static class Calendars
    {
        public const string Create = "calendar:create";
        public const string Read = "calendar:read";
        public const string Update = "calendar:update";
        public const string Delete = "calendar:delete";
    }

    public static class CalendarHolidays
    {
        public const string Create = "calendar-holiday:create";
        public const string Read = "calendar-holiday:read";
        public const string Update = "calendar-holiday:update";
        public const string Delete = "calendar-holiday:delete";
    }

    public static class Resources
    {
        public const string Create = "resource:create";
        public const string Read = "resource:read";
        public const string Update = "resource:update";
        public const string Delete = "resource:delete";
    }

    public static class ResourceSchedules
    {
        public const string Create = "resource-schedule:create";
        public const string Read = "resource-schedule:read";
        public const string Update = "resource-schedule:update";
        public const string Delete = "resource-schedule:delete";
    }

    public static class ResourceTypes
    {
        public const string Create = "resource-type:create";
        public const string Read = "resource-type:read";
        public const string Update = "resource-type:update";
        public const string Delete = "resource-type:delete";
    }

    public static class Services
    {
        public const string Create = "service:create";
        public const string Read = "service:read";
        public const string Update = "service:update";
        public const string Delete = "service:delete";
    }

    public static class Users
    {
        public const string Create = "user:create";
        public const string Read = "user:read";
        public const string Update = "user:update";
        public const string Delete = "user:delete";
    }

    public static class UsersTokens
    {
        public const string Create = "user-tokens:create";
        public const string Read = "user-tokens:read";
        public const string Update = "user-tokens:update";
        public const string Delete = "user-tokens:delete";
    }

    public static class Roles
    {
        public const string Create = "role:create";
        public const string Read = "role:read";
        public const string Update = "role:update";
        public const string Delete = "role:delete";
    }

    public static class Permissions
    {
        public const string Create = "permission:create";
        public const string Read = "permission:read";
        public const string Update = "permission:update";
        public const string Delete = "permission:delete";
    }
}
