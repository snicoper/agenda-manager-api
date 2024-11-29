namespace AgendaManager.Application.Common.Http.OrderBy.Exceptions;

public class OrderFieldEntityNotFoundException(string name, object key)
    : Exception($"""Entity "{name}" ({key}) was not found for ordering.""");
