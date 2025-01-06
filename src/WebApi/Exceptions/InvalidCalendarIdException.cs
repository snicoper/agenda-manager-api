namespace AgendaManager.WebApi.Exceptions;

public class InvalidCalendarIdException : Exception
{
    public InvalidCalendarIdException(string message)
        : base(message)
    {
    }
}
