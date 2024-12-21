using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public sealed class UserProfile : AuditableEntity
{
    private const int MaxFirstNameLength = 100;
    private const int MaxLastNameLength = 100;
    private const int MinLength = 1;

    private UserProfile()
    {
    }

    private UserProfile(
        UserProfileId userProfileId,
        UserId userId,
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber,
        Address? address,
        IdentityDocument? identityDocument)
    {
        Id = userProfileId;
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Address = address;
        IdentityDocument = identityDocument;
    }

    public UserProfileId Id { get; } = null!;

    public UserId UserId { get; } = null!;

    public User User { get; private set; } = null!;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public PhoneNumber? PhoneNumber { get; private set; }

    public Address? Address { get; private set; }

    public IdentityDocument? IdentityDocument { get; private set; }

    internal static UserProfile Create(
        UserProfileId userProfileId,
        UserId userId,
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber,
        Address? address,
        IdentityDocument? identityDocument)
    {
        ArgumentNullException.ThrowIfNull(userProfileId);
        ArgumentNullException.ThrowIfNull(userId);

        GuardAgainstInvalidFirstName(firstName);
        GuardAgainstInvalidLastName(lastName);

        var userProfile = new UserProfile(
            userProfileId,
            userId,
            firstName,
            lastName,
            phoneNumber,
            address,
            identityDocument);

        userProfile.AddDomainEvent(new UserProfileCreatedDomainEvent(userId));

        return userProfile;
    }

    internal void Update(
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber,
        Address? address,
        IdentityDocument? identityDocument)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        GuardAgainstInvalidFirstName(firstName);
        GuardAgainstInvalidLastName(lastName);

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Address = address;
        IdentityDocument = identityDocument;
    }

    internal bool HasChanges(
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber,
        Address? address,
        IdentityDocument? identityDocument)
    {
        return firstName != FirstName
               || lastName != LastName
               || !Equals(phoneNumber, PhoneNumber)
               || !Equals(address, Address)
               || !Equals(identityDocument, IdentityDocument);
    }

    private static void GuardAgainstInvalidFirstName(string firstName)
    {
        ArgumentNullException.ThrowIfNull(firstName);

        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new UserProfileDomainException("First name cannot be empty or whitespace.");
        }

        if (firstName.Length is < MinLength or > MaxFirstNameLength)
        {
            throw new UserProfileDomainException(
                $"First name must be between {MinLength} and {MaxFirstNameLength} characters.");
        }

        // Validar que no contenga números ni caracteres especiales.
        if (!firstName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new UserProfileDomainException("First name can only contain letters and spaces.");
        }
    }

    private static void GuardAgainstInvalidLastName(string lastName)
    {
        ArgumentNullException.ThrowIfNull(lastName);

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new UserProfileDomainException("Last name cannot be empty or whitespace.");
        }

        if (lastName.Length is < MinLength or > MaxLastNameLength)
        {
            throw new UserProfileDomainException(
                $"Last name must be between {MinLength} and {MaxLastNameLength} characters.");
        }

        // Validar que no contenga números ni caracteres especiales.
        if (!lastName.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
        {
            throw new UserProfileDomainException("Last name can only contain letters and spaces.");
        }
    }
}
