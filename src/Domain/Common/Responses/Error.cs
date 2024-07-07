﻿using System.Collections.ObjectModel;
using AgendaManager.Domain.Common.Extensions;

namespace AgendaManager.Domain.Common.Responses;

public class Error
{
    private readonly List<ValidationError> _validationErrors = [];

    private Error()
    {
    }

    private Error(List<ValidationError> validationErrors)
    {
        ResultType = ResultType.Validation;

        _validationErrors = validationErrors;
    }

    private Error(string code, string description, ResultType resultType = ResultType.Validation)
    {
        var validationError = new ValidationError(code, description);
        ResultType = resultType;

        _validationErrors.Add(validationError);
    }

    public ReadOnlyCollection<ValidationError> ValidationErrors => _validationErrors.AsReadOnly();

    public ResultType ResultType { get; private set; } = ResultType.Succeeded;

    public bool HasErrors => ValidationErrors.Count > 0;

    public static implicit operator Result(Error error)
    {
        return error.HasErrors ? Result.Failure(error) : Result.Success();
    }

    public static Error None()
    {
        return new Error { ResultType = ResultType.Succeeded };
    }

    public static Error Validation(string code, string description)
    {
        return new Error(code, description);
    }

    public static Error Validation(List<ValidationError> validationErrors)
    {
        return new Error(validationErrors);
    }

    public static Error NotFound(string code = "NotFound", string description = "Not Found")
    {
        return new Error(code, description, ResultType.NotFound);
    }

    public static Error Unauthorized(string code = "Unauthorized", string description = "Unauthorized")
    {
        return new Error(code, description, ResultType.Unauthorized);
    }

    public static Error Forbidden(string code = "Forbidden", string description = "Forbidden")
    {
        return new Error(code, description, ResultType.Forbidden);
    }

    public static Error Conflict(string code = "Conflict", string description = "Conflict")
    {
        return new Error(code, description, ResultType.Conflict);
    }

    public static Error Unexpected(string code = "Unexpected", string description = "Internal Server Error")
    {
        return new Error(code, description, ResultType.Unexpected);
    }

    public void AddValidationError(string code, string description)
    {
        AddValidationError(new ValidationError(code, description));
    }

    public void AddValidationError(ValidationError validationError)
    {
        ResultType = ResultType.Validation;

        _validationErrors.Add(validationError);
    }

    public ValidationError? First()
    {
        return _validationErrors.FirstOrDefault();
    }

    public ValidationError? Last()
    {
        return _validationErrors.LastOrDefault();
    }

    public ReadOnlyDictionary<string, string[]> ToDictionary()
    {
        return _validationErrors
            .GroupBy(e => e.Code, e => e.Description)
            .ToDictionary(error => error.Key.ToLowerFirstLetter(), error => error.ToArray())
            .AsReadOnly();
    }

    public Result ToResult()
    {
        return HasErrors ? Result.Failure(this) : Result.Success();
    }

    public Result<TValue> ToResult<TValue>()
    {
        return HasErrors ? Result.Failure<TValue>(this) : Result.Success<TValue>(default);
    }
}
