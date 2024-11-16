namespace AgendaManager.Domain.Calendars.Constants;

public static class CalendarConfigurationKeys
{
    /// <summary>
    /// Strategy for creating a holiday in the calendar.
    /// </summary>
    public static class HolidayCreateStrategy
    {
        public const string Key = nameof(HolidayCreateStrategy);
        public const string DefaultValue = nameof(RejectIfOverlapping);

        /// <summary>
        /// Reject if overlapping with existing appointments.
        /// </summary>
        public const string RejectIfOverlapping = nameof(RejectIfOverlapping);

        public const string RejectIfOverlappingDescription = "Reject if overlapping";

        /// <summary>
        /// Change the status of overlapping appointments to reschedule.
        /// </summary>
        public const string CancelOverlapping = nameof(CancelOverlapping);

        public const string CancelOverlappingDescription = "Cancel overlapping";

        /// <summary>
        /// Allow overlapping with existing appointments.
        /// </summary>
        public const string AllowOverlapping = nameof(AllowOverlapping);

        public const string AllowOverlappingDescription = "Allow overlapping";
    }

    /// <summary>
    /// Determines the status an appointment should have when created.
    /// </summary>
    public static class AppointmentCreationStrategy
    {
        public const string Key = nameof(AppointmentCreationStrategy);
        public const string DefaultValue = nameof(Direct);

        /// <summary>
        /// Appointments are created with the "Accepted" status by default.
        /// </summary>
        public const string Direct = nameof(Direct);

        public const string DirectDescription = "Direct";

        /// <summary>
        /// Appointments are created with a "Pending" status
        /// until the customer confirms the appointment.
        /// </summary>
        public const string RequireConfirmation = nameof(RequireConfirmation);

        public const string RequireConfirmationDescription = "Require confirmation";
    }

    /// <summary>
    /// Behavior when creating an appointment regarding overlapping with existing ones.
    /// </summary>
    public static class AppointmentOverlappingStrategy
    {
        public const string Key = nameof(AppointmentOverlappingStrategy);
        public const string DefaultValue = nameof(RejectIfOverlapping);

        /// <summary>
        /// Do not allow overlapping with existing appointments.
        /// </summary>
        public const string RejectIfOverlapping = nameof(RejectIfOverlapping);

        public const string RejectIfOverlappingDescription = "Reject if overlapping";

        /// <summary>
        /// Allow overlapping with existing appointments.
        /// </summary>
        public const string AllowOverlapping = nameof(AllowOverlapping);

        public const string AllowOverlappingDescription = "Allow overlapping";
    }

    /// <summary>
    /// Configuration for time zones.
    /// </summary>
    public static class TimeZone
    {
        /// <summary>
        /// For custom fields like strings, the Key must be "UnitValue".
        /// </summary>
        public const string Key = "UnitValue";

        public const string DefaultValue = "Europe/Madrid";

        /// <summary>
        /// String representing the time zone value in IANA format.
        /// </summary>
        public const string IanaTimeZone = nameof(IanaTimeZone);

        public const string IanaTimeZoneDescription = "Time zone";
    }
}
