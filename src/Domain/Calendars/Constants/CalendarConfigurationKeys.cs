namespace AgendaManager.Domain.Calendars.Constants;

public static class CalendarConfigurationKeys
{
    /// <summary>
    /// Estrategia al crear un día festivo de un calendario.
    /// </summary>
    public static class HolidayCreateStrategy
    {
        /// <summary>
        /// Rechazar si se solapa con citas ya creadas.
        /// </summary>
        public const string RejectIfOverlapping = nameof(RejectIfOverlapping);

        /// <summary>
        /// Cambiar estado de la cita a reprogramar si se solapa con citas ya creadas.
        /// </summary>
        public const string CancelOverlapping = nameof(CancelOverlapping);

        /// <summary>
        /// Permitir solapamiento si se solapa con citas ya creadas.
        /// </summary>
        public const string AllowOverlapping = nameof(AllowOverlapping);
    }

    /// <summary>
    /// Indica que estado debe tener una cita cuando es creada.
    /// </summary>
    public static class AppointmentCreationStrategy
    {
        /// <summary>
        /// Las citas al ser creadas, deben tener el estado "Accepted".
        /// </summary>
        public const string Direct = nameof(Direct);

        /// <summary>
        /// Las citas al ser creadas, deben tener un estado "Pending",
        /// hasta que el customer acepte la cita.
        /// </summary>
        public const string RequireConfirmation = nameof(RequireConfirmation);
    }

    /// <summary>
    /// Comportamiento al crear una cita respecto a solapamiento con citas ya creadas.
    /// </summary>
    public static class AppointmentOverlappingStrategy
    {
        /// <summary>
        /// No permitir solapamiento con citas ya creadas.
        /// </summary>
        public const string RejectIfOverlapping = nameof(RejectIfOverlapping);

        /// <summary>
        /// Permitir solapamiento con citas ya creadas.
        /// </summary>
        public const string AllowOverlapping = nameof(AllowOverlapping);
    }

    /// <summary>
    /// Valores custom de las configuraciones de un calendario.
    /// Solo tienen una única opción.
    /// </summary>
    public static class CustomValues
    {
        /// <summary>
        /// Los campos con valores custom como strings, el Key debe tener un valor "Value".
        /// </summary>
        public const string Key = "Value";

        /// <summary>
        /// String que representa el valor de la zona horaria en formato IANA.
        /// </summary>
        public const string IanaTimeZone = nameof(IanaTimeZone);
    }
}
