using AgendaManager.Application.Common.Interfaces.Messaging;
using MediatR;

namespace AgendaManager.Application.Pruebas;

public record PruebaRequest() : IQuery<Unit>;
