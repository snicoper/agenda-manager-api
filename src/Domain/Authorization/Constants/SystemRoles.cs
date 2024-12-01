namespace AgendaManager.Domain.Authorization.Constants;

/// <summary>
/// Predefined system roles that cannot be modified.
/// </summary>
public static class SystemRoles
{
    /// <summary>
    /// System administrator. Has full access to the system.
    /// </summary>
    public const string Administrator = nameof(Administrator);

    /// <summary>
    /// Internal company employee. Has access to basic system functionalities.
    /// </summary>
    public const string Employee = nameof(Employee);

    /// <summary>
    /// User who receives the services.
    /// </summary>
    public const string Customer = nameof(Customer);
}
