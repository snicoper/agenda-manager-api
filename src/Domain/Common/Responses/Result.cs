namespace AgendaManager.Domain.Common.Responses;

public record Result
{
    protected Result(Error? error)
    {
        Error = error;
        IsSuccess = error?.HasErrors is not true;
        ResultType = error?.ResultType ?? ResultType.Succeeded;
    }

    protected Result(bool isSuccess, ResultType resultType = ResultType.Succeeded)
    {
        IsSuccess = isSuccess;
        ResultType = resultType;
    }

    private Result()
    {
        IsSuccess = true;
        ResultType = ResultType.Succeeded;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ResultType ResultType { get; private init; }

    public Error? Error { get; protected init; }

    public static implicit operator Result(Error error)
    {
        return error.HasErrors ? Failure(error) : Success();
    }

    public static Result Create()
    {
        return new Result { ResultType = ResultType.Created };
    }

    public static Result<TValue> Create<TValue>(TValue? value)
    {
        return new Result<TValue>(value, null) { ResultType = ResultType.Created };
    }

    public static Result Success()
    {
        return new Result();
    }

    public static Result<TValue> Success<TValue>()
    {
        return new Result<TValue>(true);
    }

    public static Result<TValue> Success<TValue>(TValue? value, ResultType status = ResultType.Succeeded)
    {
        return new Result<TValue>(value, null) { ResultType = status };
    }

    public static Result Failure(ResultType status = ResultType.Conflict)
    {
        return new Result(false, status);
    }

    public static Result<TValue> Failure<TValue>(ResultType status = ResultType.Conflict)
    {
        return new Result<TValue>(false, status);
    }

    public static Result Failure(Error? error)
    {
        return new Result(error);
    }

    public static Result<TValue> Failure<TValue>(Error? error)
    {
        return new Result<TValue>(default, error);
    }

    public static Result NoContent()
    {
        return new Result(true, ResultType.NoContent);
    }

    public static Result<TValue> NoContent<TValue>()
    {
        return new Result<TValue>(true, ResultType.NoContent);
    }

    public Result<TValue> MapToValue<TValue>(TValue? value = default)
    {
        return new Result<TValue>(IsSuccess, ResultType) { Value = value, Error = Error };
    }
}

#pragma warning disable SA1402 // File may only contain a single type
public record Result<TValue> : Result
{
    public Result(TValue? value, Error? error)
        : base(error)
    {
        Value = value;
    }

    protected internal Result(bool isSuccess, ResultType resultType = ResultType.Succeeded)
        : base(isSuccess, resultType)
    {
    }

    public TValue? Value { get; protected internal init; }

    public bool HasValue => !EqualityComparer<TValue>.Default.Equals(Value, default);

    public static implicit operator Result<TValue>(TValue? value)
    {
        return new Result<TValue>(value, null);
    }

    public static implicit operator Result<TValue>(Error error)
    {
        return error.HasErrors ? Failure<TValue>(error) : Success<TValue>();
    }

    public Result<TDestination> MapTo<TDestination>()
    {
        var result = new Result<TDestination>(IsSuccess, ResultType) { Error = Error, Value = default };

        return result;
    }
}
#pragma warning disable SA1402 // File may only contain a single type
