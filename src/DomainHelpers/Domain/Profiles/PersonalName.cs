namespace DomainHelpers.Domain.Profiles;

public record PersonalName(string FirstName, string LastName) {
    public string? MiddleName { get; init; }

    public string FullName => $"{FirstName} {LastName}";
}