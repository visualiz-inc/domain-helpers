namespace DomainHelpers.Commons.Query; 
public class SpaceSeparatedQueryString {
    /// <summary>
    /// The separator character used for splitting query strings.
    /// </summary>
    public const char Separator = ' ';

    /// <summary>
    /// Gets the queries as an immutable array of strings.
    /// </summary>
    public ImmutableArray<string> Queries { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommaSeparatedQueryString"/> class.
    /// </summary>
    /// <param name="csv">The space-separated query string.</param>
    public SpaceSeparatedQueryString(string? csv) {
        if (csv?.Replace("　", " ").Split(Separator) is not [""] and { Length: > 0 } texts) {
            Queries = [.. texts];
        }
        else {
            Queries = ImmutableArray.Create<string>();
        }
    }

    /// <summary>
    /// Returns a string that represents the current comma-separated query string.
    /// </summary>
    /// <returns>A string that represents the current comma-separated query string.</returns>
    public override string ToString() {
        return string.Join(",", Queries);
    }

    public static SpaceSeparatedQueryString Parse(string? csv) {
        return new(csv);
    }
}