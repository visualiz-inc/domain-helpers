using System.Collections.Immutable;

namespace DomainHelpers.AspNetCore.Mediator;

public record RequestFailed(ImmutableArray<string> Details);