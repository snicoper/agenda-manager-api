namespace AgendaManager.Domain.Common.Responses;

public class Result
{
    public Result()
    {
        Succeeded = true;
        ResultType = ResultType.Succeeded;
    }

    protected Result(Error? error)
    {
        Error = error;
        Succeeded = error?.HasErrors is not true;
        ResultType = error?.ResultType ?? ResultType.Succeeded;
    }

    protected Result(bool succeeded, ResultType resultType = ResultType.Succeeded)
    {
        Succeeded = succeeded;
        ResultType = resultType;
    }

    public bool Succeeded { get; }

    public ResultType ResultType { get; private init; }

    public Error? Error { get; protected init; }

    public bool HasValue => false;

    public static Result Create()
    {
        return new Result { ResultType = ResultType.Created };
    }

    public static Result<TValue> Create<TValue>(TValue? value)
    {
        return new Result<TValue>(value, default) { ResultType = ResultType.Created };
    }

    public static Result Success(ResultType status = ResultType.Succeeded)
    {
        return new Result { ResultType = status };
    }

    public static Result<TValue> Success<TValue>(TValue? value, ResultType status = ResultType.Succeeded)
    {
        return new Result<TValue>(value, default) { ResultType = status };
    }

    public static Result Failure(ResultType status = ResultType.Conflict)
    {
        return new Result(false, status);
    }

    public static Result<TValue> Failure<TValue>(ResultType status = ResultType.Conflict)
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

    protected internal Result(bool succeeded, ResultType resultType = ResultType.Succeeded)
        : base(succeeded, resultType)
    {
    }

    public TValue? Value { get; }

    public new bool HasValue => !EqualityComparer<TValue>.Default.Equals(Value, default);

    public static implicit operator Result<TValue>(TValue? value)
    {
        return new Result<TValue>(value, default);
    }

    public Result<TDestination> MapTo<TDestination>()
    {
        var result = new Result<TDestination>(Succeeded, ResultType) { Error = Error };

        return result;
    }
}
#pragma warning disable SA1402 // File may only contain a single type
