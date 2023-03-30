namespace DomainHelpers.Commons.Tasks;

public static class TaskExtensions {
    public static Task<T> AsTask<T>(this T obj) {
        return Task.FromResult(obj);
    }
}
