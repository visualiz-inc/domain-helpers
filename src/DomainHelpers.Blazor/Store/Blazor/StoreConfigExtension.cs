using DomainHelpers.Blazor.Store.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DomainHelpers.Blazor.Store.Blazor;

public static class StoreConfigExtension {
    public static IServiceCollection AddMemento(this IServiceCollection services, bool isScoped = true) {
        if (isScoped) {
            services.AddScoped<StoreProvider>();
        }
        else {
            services.AddSingleton<StoreProvider>();
        }
        return services;
    }

    public static IServiceCollection AddStore<TStore>(this IServiceCollection collection)
        where TStore : class, IStore {
        collection.AddScoped<TStore>()
            .AddScoped<IStore>(p => p.GetRequiredService<TStore>());
        return collection;
    }

    public static void ScanAssemblyAndAddStores(this IServiceCollection services, Assembly assembly, bool isScoped = true) {
        foreach (var type in assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IStore)))) {
            if (isScoped) {
                services.AddScoped(type)
                    .AddScoped(p => (IStore)p.GetRequiredService(type));
            }
            else {
                services.AddSingleton(type)
                    .AddSingleton(p => (IStore)p.GetRequiredService(type));
            }
        }
    }

    public static IServiceCollection AddMiddleware<TMiddleware>(
        this IServiceCollection collection,
        Func<TMiddleware> middlewareSelector
    ) where TMiddleware : Middleware {
        collection.AddScoped<Middleware>(p => middlewareSelector());
        return collection;
    }

    public static IServiceCollection AddMiddleware<TMiddleware>(this IServiceCollection collection)
        where TMiddleware : Middleware {
        collection.AddScoped<TMiddleware>();
        collection.AddScoped<Middleware, TMiddleware>(p => p.GetRequiredService<TMiddleware>());
        return collection;
    }

    public static StoreProvider GetStoreProvider(this IServiceProvider provider) {
        return provider.GetRequiredService<StoreProvider>();
    }
}