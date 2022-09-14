namespace System.Collections.Generic; 

public class Map<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : notnull {
    public new TValue? this[TKey key] {
        get => TryGetValue(key, out TValue? v) ? v : default;
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