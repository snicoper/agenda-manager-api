using Microsoft.AspNetCore.Http;

namespace AgendaManager.Domain.Common.Abstractions;

public class Result
{
    public Result()
    {
        Succeeded = true;
        Status = StatusCodes.Status200OK;
    }

    protected Result(Error? error)
    {
        Error = error;
        Succeeded = error?.HasErrors is not true;
        Status = error?.Status ?? StatusCodes.Status200OK;
    }

    protected Result(bool succeeded, int status = StatusCodes.Status200OK)
    {
        Succeeded = succeeded;
        Status = status;
    }

    public bool Succeeded { get; }

    public int Status { get; private init; }

    public Error? Error { get; protected init; }

    public static Result Create(int status = StatusCodes.Status200OK)
    {
        return new Result { Status = status };
    }

    public static Result<TValue> Create<TValue>(TValue? value, int status = StatusCodes.Status200OK)
    {
        return new Result<TValue>(value, default) { Status = status };
    }

    public static Result Success(int status = StatusCodes.Status200OK)
    {
        return new Result { Status = status };
    }

    public static Result<TValue> Success<TValue>(TValue? value, int status = StatusCodes.Status200OK)
    {
        return new Result<TValue>(value, default) { Status = status };
    }

    public static Result Failure(int status = StatusCodes.Status409Conflict)
    {
        return new Result(false, status);
    }

    public static Result<TValue> Failure<TValue>(int status = StatusCodes.Status409Conflict)
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

    protected internal Result(bool succeeded, int status = StatusCodes.Status200OK)
        : base(succeeded, status)
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
        var result = new Result<TDestination>(Succeeded, Status) { Error = Error };

        return result;
    }
}
#pragma warning disable SA1402 // File may only contain a single type
