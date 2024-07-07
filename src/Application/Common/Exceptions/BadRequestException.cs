using AgendaManager.Domain.Common.Extensions;
using FluentValidation.Results;

namespace AgendaManager.Application.Common.Exceptions;

public class BadRequestException() : Exception("One or more validation failures have occurred.")
{
    public BadRequestException(IReadOnlyCollection<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key.ToLowerFirstLetter(), failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
}
