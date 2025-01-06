namespace AgendaManager.WebApi.Exceptions;

public class CalendarNotSelectedException : Exception
{
    public CalendarNotSelectedException(string message)
        : base(message)
    {
    }
}
