namespace AgendaManager.Domain.Common.Abstractions;

public class Result
{
}

#pragma warning disable SA1402 // File may only contain a single type
public abstract class Result<TValue> : Result
{
}
#pragma warning disable SA1402 // File may only contain a single type
