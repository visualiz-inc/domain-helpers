using DomainHelpers.Commons;
using DomainHelpers.Commons.Text;

namespace DomainHelpers.Domain.Indentifier; 
public abstract class IdentifierCode {
    public IdentifierCode(string src) {
        Values = src switch {
            var s when Validate(s) => Parse(s),
            _ => throw new ArgumentException("srcは0-9の数値または-のみで構成されている必要があります。", nameof(src))
        };
    }

    public abstract string Vocabs { get; }

    public abstract string Separator { get; }

    public abstract ImmutableArray<int> Digits { get; }

    public abstract string Prefix { get; }

    public int CharacterLengthWithoutSeparator => Digits.Aggregate(0, (x, y) => x + y);

    public int TotalCharcterLength => Digits.Aggregate(0, (x, y) => x + y + Separator.Length);

    public string Pattern => Digits
        .Select(d => "x".Repeat(d))
        .JoinStrings(Separator);

    protected ImmutableArray<string> Values { get; set; }

    public string? this[int i] => i < Values.Length ? Values[i] : null;

    protected virtual bool Validate(string src) {
        foreach (char c in src) {
            if (Vocabs.Contains(c) is false) {
                return false;
            }
        }

        return true;
    }

    protected virtual ImmutableArray<string> Parse(string source) {
        string[] blocks = source.Split(Separator);

        foreach (int i in ..Vocabs.Length) {
            if (i >= blocks.Length
                || blocks[i].Length != Vocabs[i]
                || CheckVocabContains(blocks[i]) is false) {
                throw new Exception();
            }
        }

        return [.. blocks];
    }

    private bool CheckVocabContains(string text) {
        foreach (char c in text) {
            if (Vocabs.Contains(c) is false) {
                return false;
            }
        }

        return true;
    }

    public override string ToString() {
        return Values.JoinStrings(Separator);
    }
}