namespace AgendaManager.Domain.Calendars.Configurations;

public static class CalendarConfigurationKeys
{
    public static class Appointments
    {
        // Determines the creation strategy for appointments.
        public const string ConfirmationStrategy = "AppointmentConfirmationStrategy";

        // Determines the overlapping strategy for appointments.
        public const string OverlappingStrategy = "AppointmentOverlappingStrategy";

        public static class ConfirmationOptions
        {
            public const string RequireConfirmation = nameof(RequireConfirmation);
            public const string AutoAccept = nameof(AutoAccept);
        }

        public static class OverlappingOptions
        {
            public const string AllowOverlapping = nameof(AllowOverlapping);
            public const string RejectIfOverlapping = nameof(RejectIfOverlapping);
        }
    }

    public static class ResourcesSchedules
    {
        // Determines the resources schedule validation strategy for appointments.
        public const string AvailabilityStrategy = "ResourceAvailabilityStrategy";

        public static class AvailabilityOptions
        {
            public const string ValidateSchedules = nameof(ValidateSchedules);
            public const string IgnoreSchedules = nameof(IgnoreSchedules);
        }
    }

    public static class Holidays
    {
        // Determines the creation strategy for holidays.
        public const string ConflictStrategy = "HolidayConflictStrategy";

        public static class ConflictOptions
        {
            public const string RejectIfOverlapping = nameof(RejectIfOverlapping);
            public const string CancelOverlapping = nameof(CancelOverlapping);
            public const string AllowOverlapping = nameof(AllowOverlapping);
        }
    }

    // Determines the time zone of the calendar.
    public static class TimeZone
    {
        public const string Category = "IanaTimeZone";
        public const string Key = "UnitValue";
    }

    public static class Metadata
    {
        public static readonly IReadOnlyDictionary<string, ConfigurationOption> Options =
            new Dictionary<string, ConfigurationOption>
            {
                // Appointment Creation Strategy.
                [Appointments.ConfirmationStrategy] = new(
                    category: Appointments.ConfirmationStrategy,
                    defaultKey: Appointments.ConfirmationOptions.AutoAccept,
                    availableKeys:
                    [
                        Appointments.ConfirmationOptions.AutoAccept,
                        Appointments.ConfirmationOptions.RequireConfirmation
                    ],
                    description: "Defines how appointments are created"),

                // Appointment Overlapping Strategy.
                [Appointments.OverlappingStrategy] = new(
                    category: Appointments.OverlappingStrategy,
                    defaultKey: Appointments.OverlappingOptions.RejectIfOverlapping,
                    availableKeys:
                    [
                        Appointments.OverlappingOptions.RejectIfOverlapping,
                        Appointments.OverlappingOptions.AllowOverlapping
                    ],
                    description: "Defines how overlapping appointments are handled"),

                // Appointment Resources Schedule Validation Strategy.
                [ResourcesSchedules.AvailabilityStrategy] = new(
                    category: ResourcesSchedules.AvailabilityStrategy,
                    defaultKey: ResourcesSchedules.AvailabilityOptions.ValidateSchedules,
                    availableKeys:
                    [
                        ResourcesSchedules.AvailabilityOptions.ValidateSchedules,
                        ResourcesSchedules.AvailabilityOptions.IgnoreSchedules
                    ],
                    description: "Defines how resources schedule validation is handled"),

                // Holiday Creation Strategy.
                [Holidays.ConflictStrategy] = new(
                    category: Holidays.ConflictStrategy,
                    defaultKey: Holidays.ConflictOptions.RejectIfOverlapping,
                    availableKeys:
                    [
                        Holidays.ConflictOptions.RejectIfOverlapping,
                        Holidays.ConflictOptions.CancelOverlapping,
                        Holidays.ConflictOptions.AllowOverlapping
                    ],
                    description: "Defines how holidays interact with existing appointments"),

                // TimeZone (UnitValue type).
                [TimeZone.Category] = new(
                    category: TimeZone.Category,
                    isUnitValue: true,
                    validator: _ => true,
                    description: "Calendar timezone in IANA format")
            };

        public static bool IsValidConfiguration(string category, string selectedKey)
        {
            if (!Options.TryGetValue(category, out var option))
            {
                return false;
            }

            if (option.IsUnitValue)
            {
                return option.Validator?.Invoke(selectedKey) ?? false;
            }

            return option.AvailableKeys.Contains(selectedKey);
        }

        public static string GetDefaultKey(string category)
        {
            if (!Options.TryGetValue(category, out var option))
            {
                throw new InvalidOperationException($"Unknown configuration category: {category}");
            }

            if (option.IsUnitValue)
            {
                throw new InvalidOperationException($"UnitValue configurations don't have default keys: {category}");
            }

            return option.DefaultKey!;
        }
    }
}
