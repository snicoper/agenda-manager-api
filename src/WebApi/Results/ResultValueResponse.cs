using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.WebApi.Results;

public record ResultValueResponse<TValue>(TValue? Value, bool IsSuccess, ResultType ResultType);
