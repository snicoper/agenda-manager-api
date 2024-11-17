namespace AgendaManager.Domain.Calendars.Configurations;

public static class CalendarConfigurationKeys
{
    public static class Appointments
    {
        public const string CreationStrategy = "AppointmentCreationStrategy";
        public const string OverlappingStrategy = "AppointmentOverlappingStrategy";

        public static class CreationOptions
        {
            public const string RequireConfirmation = "RequireConfirmation";
            public const string Direct = "Direct";
        }

        public static class OverlappingOptions
        {
            public const string AllowOverlapping = "AllowOverlapping";
            public const string RejectIfOverlapping = "RejectIfOverlapping";
        }
    }

    public static class Holidays
    {
        public const string CreateStrategy = "HolidayCreateStrategy";

        public static class CreationOptions
        {
            public const string RejectIfOverlapping = "RejectIfOverlapping";
            public const string CancelOverlapping = "CancelOverlapping";
            public const string AllowOverlapping = "AllowOverlapping";
        }
    }

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
                [Appointments.CreationStrategy] = new(
                    category: Appointments.CreationStrategy,
                    defaultKey: Appointments.CreationOptions.Direct,
                    availableKeys:
                    [
                        Appointments.CreationOptions.Direct,
                        Appointments.CreationOptions.RequireConfirmation
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

                // Holiday Creation Strategy.
                [Holidays.CreateStrategy] = new(
                    category: Holidays.CreateStrategy,
                    defaultKey: Holidays.CreationOptions.RejectIfOverlapping,
                    availableKeys:
                    [
                        Holidays.CreationOptions.RejectIfOverlapping,
                        Holidays.CreationOptions.CancelOverlapping,
                        Holidays.CreationOptions.AllowOverlapping
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
