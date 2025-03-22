namespace AgendaManager.Domain.Common.WeekDays.Exceptions;

public class WeekDaysException : Exception
{
    public WeekDaysException(string message)
        : base(message)
    {
    }
}
