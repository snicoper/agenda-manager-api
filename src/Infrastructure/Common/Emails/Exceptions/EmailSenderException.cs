namespace AgendaManager.Infrastructure.Common.Emails.Exceptions;

public sealed class EmailSenderException(string message, string paramName)
    : Exception($"{message} (Parameter '{paramName}')");
