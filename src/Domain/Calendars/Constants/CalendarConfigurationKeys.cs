namespace AgendaManager.Domain.Calendars.Constants;

public static class CalendarConfigurationKeys
{
    /// <summary>
    /// Estrategia al crear un día festivo de un calendario.
    /// </summary>
    public static class HolidayCreateStrategy
    {
        public const string Key = nameof(HolidayCreateStrategy);

        /// <summary>
        /// Rechazar si se solapa con citas ya creadas.
        /// </summary>
        public const string RejectIfOverlapping = nameof(RejectIfOverlapping);

        public const string RejectIfOverlappingDescription = "Reject if overlapping";

        /// <summary>
        /// Cambiar estado de la cita a reprogramar si se solapa con citas ya creadas.
        /// </summary>
        public const string CancelOverlapping = nameof(CancelOverlapping);

        public const string CancelOverlappingDescription = "Cancel overlapping";

        /// <summary>
        /// Permitir solapamiento si se solapa con citas ya creadas.
        /// </summary>
        public const string AllowOverlapping = nameof(AllowOverlapping);

        public const string AllowOverlappingDescription = "Allow overlapping";
    }

    /// <summary>
    /// Indica que estado debe tener una cita cuando es creada.
    /// </summary>
    public static class AppointmentCreationStrategy
    {
        public const string Key = nameof(AppointmentCreationStrategy);

        /// <summary>
        /// Las citas al ser creadas, deben tener el estado "Accepted".
        /// </summary>
        public const string Direct = nameof(Direct);

        public const string DirectDescription = "Direct";

        /// <summary>
        /// Las citas al ser creadas, deben tener un estado "Pending",
        /// hasta que el customer acepte la cita.
        /// </summary>
        public const string RequireConfirmation = nameof(RequireConfirmation);

        public const string RequireConfirmationDescription = "Require confirmation";
    }

    /// <summary>
    /// Comportamiento al crear una cita respecto a solapamiento con citas ya creadas.
    /// </summary>
    public static class AppointmentOverlappingStrategy
    {
        public const string Key = nameof(AppointmentOverlappingStrategy);

        /// <summary>
        /// No permitir solapamiento con citas ya creadas.
        /// </summary>
        public const string RejectIfOverlapping = nameof(RejectIfOverlapping);

        public const string RejectIfOverlappingDescription = "Reject if overlapping";

        /// <summary>
        /// Permitir solapamiento con citas ya creadas.
        /// </summary>
        public const string AllowOverlapping = nameof(AllowOverlapping);

        public const string AllowOverlappingDescription = "Allow overlapping";
    }

    /// <summary>
    /// Valores custom de las configuraciones de un calendario.
    /// Solo tienen una única opción.
    /// </summary>
    public static class CustomValues
    {
        /// <summary>
        /// Los campos con valores custom como strings, el Key debe tener un valor "UnitValue".
        /// </summary>
        public const string Key = "UnitValue";

        /// <summary>
        /// String que representa el valor de la zona horaria en formato IANA.
        /// </summary>
        public const string IanaTimeZone = nameof(IanaTimeZone);
    }
}
