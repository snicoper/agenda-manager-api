namespace AgendaManager.Domain.Common.Abstractions;

public class Error
{
}

#pragma warning disable SA1402 // File may only contain a single type
public sealed class Error<TValue> : Error
{
}
#pragma warning disable SA1402 // File may only contain a single type
