namespace AgendaManager.Domain.Common.Responses;

public class Result
{
    public Result()
    {
        Succeeded = true;
        ErrorType = ErrorType.None;
    }

    protected Result(Error? error)
    {
        Error = error;
        Succeeded = error?.HasErrors is not true;
        ErrorType = error?.ErrorType ?? ErrorType.None;
    }

    protected Result(bool succeeded, ErrorType errorType = ErrorType.None)
    {
        Succeeded = succeeded;
        ErrorType = errorType;
    }

    public bool Succeeded { get; }

    public ErrorType ErrorType { get; private init; }

    public Error? Error { get; protected init; }

    public static Result Create(ErrorType status = ErrorType.None)
    {
        return new Result { ErrorType = status };
    }

    public static Result<TValue> Create<TValue>(TValue? value, ErrorType status = ErrorType.None)
    {
        return new Result<TValue>(value, default) { ErrorType = status };
    }

    public static Result Success(ErrorType status = ErrorType.None)
    {
        return new Result { ErrorType = status };
    }

    public static Result<TValue> Success<TValue>(TValue? value, ErrorType status = ErrorType.None)
    {
        return new Result<TValue>(value, default) { ErrorType = status };
    }

    public static Result Failure(ErrorType status = ErrorType.Conflict)
    {
        return new Result(false, status);
    }

    public static Result<TValue> Failure<TValue>(ErrorType status = ErrorType.Conflict)
    {
        return new Result<TValue>(default, status);
    }

    public static Result Failure(Error? error)
    {
        return new Result(error);
    }

    public static Result<TValue> Failure<TValue>(Error? error)
    {
        return new Result<TValue>(default, error);
    }
}

#pragma warning disable SA1402 // File may only contain a single type
public class Result<TValue> : Result
{
    public Result(TValue? value)
        : base(default)
    {
        Value = value;
    }

    protected internal Result(TValue? value, Error? error)
        : base(error)
    {
        Value = value;
    }

    protected internal Result(bool succeeded, ErrorType errorType = ErrorType.None)
        : base(succeeded, errorType)
    {
    }

    public TValue? Value { get; }

    public bool HasValue => !EqualityComparer<TValue>.Default.Equals(Value, default);

    public static implicit operator Result<TValue>(TValue? value)
    {
        return new Result<TValue>(value, default);
    }

    public Result<TDestination> MapTo<TDestination>()
    {
        var result = new Result<TDestination>(Succeeded, ErrorType) { Error = Error };

        return result;
    }
}
#pragma warning disable SA1402 // File may only contain a single type
