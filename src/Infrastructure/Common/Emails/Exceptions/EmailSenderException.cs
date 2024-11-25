namespace AgendaManager.Infrastructure.Common.Emails.Exceptions;

public class EmailSenderException(string message, string paramName)
    : Exception($"{message} (Parameter '{paramName}')");
