using System.Collections;

namespace DomainHelpers.Core.Validations.Internal; 

internal class TrackingCollection<T> : IEnumerable<T> {
    private readonly List<T> _innerCollection = new();
    private Action<T>? _capture;

    public int Count => _innerCollection.Count;

    public IEnumerator<T> GetEnumerator() {
        return _innerCollection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public event Action<T>? ItemAdded;

    public void Add(T item) {
        if (_capture == null) {
            _innerCollection.Add(item);
        }
        else {
            _capture(item);
        }

        ItemAdded?.Invoke(item);
    }

    public void Remove(T item) {
        _innerCollection.Remove(item);
    }

    public IDisposable OnItemAdded(Action<T> onItemAdded) {
        ItemAdded += onItemAdded;
        return new EventDisposable(this, onItemAdded);
    }

    internal IDisposable Capture(Action<T> onItemAdded) {
        return new CaptureDisposable(this, onItemAdded);
    }

    public void AddRange(IEnumerable<T> collection) {
        _innerCollection.AddRange(collection);
    }

    private class EventDisposable : IDisposable {
        private readonly Action<T> handler;
        private readonly TrackingCollection<T> parent;

        public EventDisposable(TrackingCollection<T> parent, Action<T> handler) {
            this.parent = parent;
            this.handler = handler;
        }

        public void Dispose() {
            parent.ItemAdded -= handler;
        }
    }

    private class CaptureDisposable : IDisposable {
        private readonly Action<T>? _old;
        private readonly TrackingCollection<T> _parent;

        public CaptureDisposable(TrackingCollection<T> parent, Action<T> handler) {
            _parent = parent;
            _old = parent._capture;
            parent._capture = handler;
        }

        public void Dispose() {
            _parent._capture = _old;
        }
    }
}