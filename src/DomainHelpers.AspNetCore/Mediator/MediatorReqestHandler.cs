using DomainHelpers.Commons;
using DomainHelpers.Commons.Primitives;
using DomainHelpers.Commons.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;

namespace DomainHelpers.AspNetCore.Mediator;

public record RemoteMediatrOptions {
    public bool PerformAuthorizationByDefault { get; init; }

    public string UnknownErrorMessage { get; init; } = "Unknown request error occurred.";
}

internal class RemoteMediatrRequestHandler(
    Assembly assembly,
    IServiceScopeFactory serviceScopeFactory,
    IAuthorizationPolicyProvider? authorizationPolicyProvider,
    RemoteMediatrOptions options,
    ILogger<RemoteMediatrRequestHandler> logger,
    bool isDebug
    ) {
    private readonly Assembly _assembly = assembly;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IAuthorizationPolicyProvider? _authorizationPolicyProvider = authorizationPolicyProvider;
    private readonly RemoteMediatrOptions _options = options;
    private readonly ILogger<RemoteMediatrRequestHandler> _logger = logger;
    private readonly bool _isDebug = isDebug;

    public async Task<IResult> HandleRequest(string requestType, HttpContext context) {
        var type = (
            from t in _assembly.DefinedTypes
            from i in t.GetInterfaces()
            where t.Name == requestType
            where i.IsGenericType
            where i.GetGenericTypeDefinition() == typeof(IRequest<>)
            select t.AsType()
        ).FirstOrDefault();
        if (type is null) {
            return Results.BadRequest($"Type {requestType} was not found");
        }

        var authResult = await AuthorizeRequest(context, type);
        if (authResult is not null) {
            return Results.Unauthorized();
        }

        using var stream = new StreamReader(context.Request.Body);
        var body = await stream.ReadToEndAsync();
        var obj = JsonSerializer.Deserialize(body, type);
        if (obj is null) {
            return Results.BadRequest($"Could not convert payload to {requestType}");
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        try {
            return await mediator.Send(obj) switch {
                Unit => Results.NoContent(),
                var result => Results.Ok(result),
            };
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to handle request.");
            return ex switch {
                Error ge => Results.Problem(
                    title: ge.DisplayMessage,
                    detail: ge.FluttenDisplayMessages().JoinStrings(","),
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: new Dictionary<string, object?>() {
                        ["exception"] = _isDebug
                            ? ex.ToString()
                            : "This info is valid on development mode only",
                        ["payload"] = ge.Payload is not null ? JsonSerializer.Serialize(ge.Payload) : "",
                        ["eventId"] = ServerErrorExceptionId.Parse($"{ServerErrorExceptionId.Prefix}{ServerErrorExceptionId.Separator}{ge.EventId.Value}").ToString(),
                        ["childDetails"] = ge.Payload is RequestFailed fr
                            ? fr.Details
                            : ImmutableArray.Create<string>(),
                    }
                ),
                _ => Results.Problem(
                    title: _options.UnknownErrorMessage,
                    detail: _options.UnknownErrorMessage,
                    statusCode: StatusCodes.Status500InternalServerError,
                    extensions: new Dictionary<string, object?>() {
                        ["exception"] = _isDebug ? ex.ToString() : "Dev only",
                        ["payload"] = "",
                        ["eventId"] = ServerErrorExceptionId.Parse($"{ServerErrorExceptionId.Prefix}{ServerErrorExceptionId.Separator}{default(Ulid)}").ToString(),
                        ["childDetails"] = ImmutableArray.Create<string>(),
                    }
                ),
            };
        }
    }

    private async Task<IActionResult?> AuthorizeRequest(HttpContext httpContext, Type requestedType) {
        if (
            _authorizationPolicyProvider is null
            || requestedType.GetCustomAttribute<AllowAnonymousAttribute>() is not null
        ) {
            return null;
        }

        var authData = requestedType.GetCustomAttributes<AuthorizeAttribute>().ToList();
        if (_options.PerformAuthorizationByDefault)
            authData.Add(new AuthorizeAttribute());

        if (authData.Any() is false)
            return null;

        var authorizeFilter = new AuthorizeFilter(_authorizationPolicyProvider, authData);
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        var authorizationFilterContext = new AuthorizationFilterContext(actionContext, new[] { authorizeFilter });

        await authorizeFilter.OnAuthorizationAsync(authorizationFilterContext);

        return authorizationFilterContext.Result;
    }
}