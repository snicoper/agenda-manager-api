using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.WebApi.Results;

public record ResultResponse(bool IsSuccess, ResultType ResultType);
