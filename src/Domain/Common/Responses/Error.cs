﻿using System.Collections.ObjectModel;
using AgendaManager.Domain.Common.Extensions;

namespace AgendaManager.Domain.Common.Responses;

public class Error
{
    private readonly Dictionary<string, string[]> _validationErrors = [];

    protected Error()
    {
    }

    protected Error(Dictionary<string, string[]> validationErrors)
    {
        _validationErrors = validationErrors;

        ResultType = ResultType.Validation;
    }

    protected Error(string code, string description, ResultType resultType = ResultType.Validation)
    {
        Code = code;
        Description = description;
        ResultType = resultType;
    }

    public ReadOnlyDictionary<string, string[]> ValidationErrors => _validationErrors.AsReadOnly();

    public string Code { get; } = default!;

    public string Description { get; } = default!;

    public ResultType ResultType { get; private set; } = ResultType.Succeeded;

    public bool HasErrors => ValidationErrors.Count > 0 || Code != default! || Description != default!;

    public static implicit operator Result(Error error)
    {
        return error.HasErrors ? Result.Failure(error) : Result.Success();
    }

    public static Error None()
    {
        return new Error();
    }

    public static Error<TValue> None<TValue>()
    {
        return new Error<TValue>();
    }

    public static Error Validation(string code, string description)
    {
        var error = new Error();
        error.AddValidationError(code, description);

        return error;
    }

    public static Error<TValue> Validation<TValue>(string code, string description)
    {
        var error = new Error<TValue> { ResultType = ResultType.Validation };
        error.AddValidationError(code, description);

        return error;
    }

    public static Error Validation(Dictionary<string, string[]> validationErrors)
    {
        return new Error(validationErrors);
    }

    public static Error<TValue> Validation<TValue>(Dictionary<string, string[]> validationErrors)
    {
        return new Error<TValue>(validationErrors);
    }

    public static Error NotFound(string code, string description)
    {
        var error = $"Entity \"{code}\" ({description}) was not found.";

        return new Error(code, error, ResultType.NotFound);
    }

    public static Error<TValue> NotFound<TValue>(string code, string description)
    {
        var error = $"Entity \"{code}\" ({description}) was not found.";

        return new Error<TValue>(code, error, ResultType.NotFound);
    }

    public static Error Unauthorized(string description = "Unauthorized")
    {
        return new Error(nameof(ResultType.Unauthorized), description, ResultType.Unauthorized);
    }

    public static Error<TValue> Unauthorized<TValue>(string description = "Unauthorized")
    {
        return new Error<TValue>(nameof(ResultType.Unauthorized), description, ResultType.Unauthorized);
    }

    public static Error Forbidden(string description = "Forbidden")
    {
        return new Error(nameof(ResultType.Forbidden), description, ResultType.Forbidden);
    }

    public static Error<TValue> Forbidden<TValue>(string description = "Forbidden")
    {
        return new Error<TValue>(nameof(ResultType.Forbidden), description, ResultType.Forbidden);
    }

    public static Error Unexpected(string? code, string description = "Internal Server Error")
    {
        code ??= nameof(ResultType.Unexpected);

        return new Error(code, description, ResultType.Unexpected);
    }

    public static Error<TValue> Unexpected<TValue>(string? code, string description = "Internal Server Error")
    {
        code ??= nameof(ResultType.Unexpected);

        return new Error<TValue>(code, description, ResultType.Unexpected);
    }

    public void AddValidationError(string code, string description)
    {
        ResultType = ResultType.Validation;
        code = code.ToLowerFirstLetter();

        if (_validationErrors.TryGetValue(code, out var value))
        {
            var newValue = value.Append(description).ToArray();
            _validationErrors[code] = newValue;

            return;
        }

        _validationErrors.Add(code, [description]);
    }

    public Result ToResult()
    {
        return HasErrors ? Result.Failure(this) : Result.Success();
    }
}

#pragma warning disable SA1402 // File may only contain a single type
public class Error<TValue> : Error
{
    protected internal Error()
    {
    }

    protected internal Error(Dictionary<string, string[]> validationErrors)
        : base(validationErrors)
    {
    }

    protected internal Error(string code, string description, ResultType resultType = ResultType.Succeeded)
        : base(code, description, resultType)
    {
    }

    public static implicit operator Result<TValue>(Error<TValue> error)
    {
        return error.HasErrors ? Result.Failure<TValue>(error) : Result.Success<TValue>(default);
    }

    public new Result<TValue> ToResult()
    {
        return HasErrors ? Result.Failure<TValue>(this) : Result.Success<TValue>(default);
    }
}
#pragma warning disable SA1402 // File may only contain a single type
