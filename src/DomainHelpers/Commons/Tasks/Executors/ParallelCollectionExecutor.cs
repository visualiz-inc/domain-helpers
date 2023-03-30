namespace DomainHelpers.Commons.Tasks.Executors;

public abstract class TaskCollectionHandler {
    public static ParallelCollectionExecutor<T> Create<T>(ICollection<T> tasks, int parallelCount) {
        return new ParallelCollectionExecutor<T>(tasks, parallelCount);
    }
}

public class ParallelCollectionExecutor<T> {
    private int _progress;
    private bool _isInvoked = false;

    public int Progress => _progress;

    public int Count => Tasks.Count;

    private System.Collections.Concurrent.ConcurrentQueue<T> Tasks { get; }

    private int ParallelCount { get; }

    public TimeSpan DeleyPerStartup { get; } = TimeSpan.FromMilliseconds(100);

    public ParallelCollectionExecutor(ICollection<T> tasks, int parallelCount = 100) {
        Tasks = new System.Collections.Concurrent.ConcurrentQueue<T>(tasks);
        ParallelCount = parallelCount;
    }

    public async Task RunAsync(Func<T, Task> handler) {
        if (_isInvoked) {
            throw new InvalidOperationException("This handler is already invoked.");
        }

        _isInvoked = true;
        await Task.WhenAll(Run(handler));
    }

    private IEnumerable<Task> Run(Func<T, Task> handler) {
        foreach (var i in 0..^ParallelCount) {
            yield return Task.Run(async () => {
                await Task.Delay(DeleyPerStartup.Milliseconds * i);
                while (Tasks.TryDequeue(out var t)) {
                    Interlocked.Increment(ref _progress);
                    await handler(t);
                }
            });
        }
    }
}
