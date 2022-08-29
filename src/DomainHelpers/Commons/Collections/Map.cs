namespace System.Collections.Generic;

public class Map<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : notnull {
    public new TValue? this[TKey key] {
        get {
            return this.TryGetValue(key, out var v) ? v : default;
        }
        set {
            if (this.ContainsKey(key)) {
                this[key] = value;
            }
            else {
                this.Add(key, value!);
            }
        }
    }
}