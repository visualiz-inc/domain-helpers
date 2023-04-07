namespace DomainHelpers.Commons.Collections; 
public class NullableMap<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : notnull
    where TValue : class {
    public new TValue? this[TKey key] {
        get => TryGetValue(key, out var v) ? v : null;
        set {
            if (ContainsKey(key)) {
                this[key] = value;
            }
            else {
                Add(key, value!);
            }
        }
    }
}

public class DefaultValueMap<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : notnull {
    public new TValue this[TKey key] {
        get => TryGetValue(key, out var v) ? v : default!;
        set {
            if (ContainsKey(key)) {
                this[key] = value;
            }
            else {
                Add(key, value);
            }
        }
    }
}