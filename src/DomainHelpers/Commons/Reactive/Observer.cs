namespace DomainHelpers.Commons.Reactive;
public class Observer<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted) : IObserver<T> {
    private readonly Action onCompleted = onCompleted;
    private readonly Action<Exception> onError = onError;
    private readonly Action<T> onNext = onNext;

    public void OnCompleted() {
        onCompleted();
    }

    public void OnError(Exception error) {
        onError(error);
    }

    public void OnNext(T value) {
        onNext(value);
    }
}