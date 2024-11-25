namespace AgendaManager.Application.Common.Exceptions;

public class ApplicationEventHandlerException : Exception
{
    public ApplicationEventHandlerException(string message)
        : base(message)
    {
    }
}
