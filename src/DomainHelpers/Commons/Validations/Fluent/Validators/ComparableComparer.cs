namespace DomainHelpers.Core.Validations.Validators;

internal class ComparableComparer<T> : IComparer<T> where T : IComparable<T> {
    static ComparableComparer() {
        Instance = new ComparableComparer<T>();
    }

    internal static ComparableComparer<T> Instance { get; }

    public int Compare(T x, T y) {
        return x.CompareTo(y);
    }
}