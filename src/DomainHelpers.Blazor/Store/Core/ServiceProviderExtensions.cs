namespace DomainHelpers.Blazor.Store.Core;

public static class ServiceProviderExtensions {
    public static IEnumerable<IStore> GetAllStores(this IServiceProvider provider) {
        return provider.GetServices<IStore>();
    }

    public static IEnumerable<Middleware> GetAllMiddleware(this IServiceProvider provider) {
        return provider.GetServices<Middleware>();
    }

    public static IEnumerable<T> GetServices<T>(this IServiceProvider provider) {
        if (provider.GetService(typeof(IEnumerable<T>)) is IEnumerable<T> services) {
            return services;
        }

        throw new InvalidOperationException("Service cannot resolve.");
    }
}