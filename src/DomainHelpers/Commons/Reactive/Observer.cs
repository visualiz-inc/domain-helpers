namespace DomainHelpers.Commons.Reactive;

public class Observer<T> : IObserver<T> {
    private readonly Action onCompleted;
    private readonly Action<Exception> onError;
    private readonly Action<T> onNext;

    public Observer(Action<T> onNext, Action<Exception> onError, Action onCompleted) {
        this.onNext = onNext;
        this.onError = onError;
        this.onCompleted = onCompleted;
    }


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