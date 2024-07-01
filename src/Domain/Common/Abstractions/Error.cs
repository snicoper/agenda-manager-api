using System.Collections.ObjectModel;
using AgendaManager.Domain.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace AgendaManager.Domain.Common.Abstractions;

public class Error
{
    private readonly Dictionary<string, string[]> _validationErrors = [];

    protected Error()
    {
    }

    protected Error(string code, string description, int status = StatusCodes.Status400BadRequest)
    {
        Code = code;
        Description = description;
        Status = status;
    }

    public ReadOnlyDictionary<string, string[]> ValidationErrors => _validationErrors.AsReadOnly();

    public string Code { get; } = default!;

    public string Description { get; } = default!;

    public int Status { get; private set; } = StatusCodes.Status200OK;

    public bool HasErrors => ValidationErrors.Count > 0 || Code != default! || Description != default!;

    public static implicit operator Result(Error error)
    {
        return error.HasErrors ? Result.Failure(error) : Result.Success();
    }

    public static Error Validation(string code, string description)
    {
        var error = new Error();
        error.AddValidationError(code, description);

        return error;
    }

    public static Error<TValue> Validation<TValue>(string code, string description)
    {
        var error = new Error<TValue> { Status = StatusCodes.Status400BadRequest };
        error.AddValidationError(code, description);

        return error;
    }

    public static Error NotFound(string code, string description)
    {
        var error = $"Entity \"{code}\" ({description}) was not found.";

        return new Error(code, error, StatusCodes.Status404NotFound);
    }

    public static Error<TValue> NotFound<TValue>(string code, string description)
    {
        var error = $"Entity \"{code}\" ({description}) was not found.";

        return new Error<TValue>(code, error, StatusCodes.Status404NotFound);
    }

    public static Error Unauthorized(string description = "Unauthorized")
    {
        return new Error(nameof(StatusCodes.Status401Unauthorized), description, StatusCodes.Status401Unauthorized);
    }

    public static Error<TValue> Unauthorized<TValue>(string description = "Unauthorized")
    {
        return new Error<TValue>(nameof(StatusCodes.Status401Unauthorized), description, StatusCodes.Status401Unauthorized);
    }

    public static Error Forbidden(string description = "Forbidden")
    {
        return new Error(nameof(StatusCodes.Status403Forbidden), description, StatusCodes.Status403Forbidden);
    }

    public static Error<TValue> Forbidden<TValue>(string description = "Forbidden")
    {
        return new Error<TValue>(nameof(StatusCodes.Status403Forbidden), description, StatusCodes.Status403Forbidden);
    }

    public void AddValidationError(string code, string description)
    {
        Status = StatusCodes.Status400BadRequest;
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

    protected internal Error(string code, string description, int status = StatusCodes.Status400BadRequest)
        : base(code, description, status)
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
