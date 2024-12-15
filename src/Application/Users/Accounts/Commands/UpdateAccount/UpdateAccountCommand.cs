﻿using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.Application.Users.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    UpdateAccountCommand.PhoneCommand Phone,
    UpdateAccountCommand.AddressCommand Address,
    UpdateAccountCommand.IdentityDocumentCommand IdentityDocument) : ICommand
{
    public record PhoneCommand(string Number, string CountryCode);

    public record AddressCommand(string Street, string City, string Country, string State, string PostalCode);

    public record IdentityDocumentCommand(string Number, string CountryCode, IdentityDocumentType Type);
}
