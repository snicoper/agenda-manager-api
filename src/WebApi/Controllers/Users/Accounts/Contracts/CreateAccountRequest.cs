﻿namespace AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;

public record CreateAccountRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string PasswordConfirmation,
    List<Guid> Roles,
    bool IsActive,
    bool IsEmailConfirmed,
    bool IsCollaborator);
