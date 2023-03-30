using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DomainHelpers.AspNetCore.Mediator;

public static class RemoteMediatrServiceBuilder {
    private const string _requestPath = "mediator";

    public static void MapRemoteMediatrListener(
        this WebApplication app,
        Assembly assembly,
        Func<RemoteMediatrOptions, RemoteMediatrOptions>? optionsBuilder = null
    ) {
        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        var policyProvider = app.Services.GetService<IAuthorizationPolicyProvider>();

        var options = optionsBuilder?.Invoke(new RemoteMediatrOptions())
            ?? new RemoteMediatrOptions();

        var requestHandler = new RemoteMediatrRequestHandler(
            assembly,
            scopeFactory,
            policyProvider,
            options,
            app.Services.GetRequiredService<ILogger<RemoteMediatrRequestHandler>>(),
            app.Environment.IsDevelopment()
        );

        app.MapPost($"{_requestPath}/{{requestType}}", requestHandler.HandleRequest);
    }
}