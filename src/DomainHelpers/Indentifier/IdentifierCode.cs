namespace DomainHelpers.Indentifier;

public abstract class IdentifierCode {
    public abstract string Vocabs { get; }

    public abstract string Separator { get; }

    public abstract ImmutableArray<int> Digits { get; }

    public abstract string Prefix { get; }

    public int CharacterLengthWithoutSeparator => this.Digits.Aggregate(0, (x, y) => x + y);

    public int TotalCharcterLength => this.Digits.Aggregate(0, (x, y) => x + y + this.Separator.Length);

    public string Pattern => this.Digits
        .Select(d => "x".Repeat(d))
        .JoinStrings(this.Separator);

    protected ImmutableArray<string> Values { get; set; }

    public IdentifierCode(string src) {
        this.Values = src switch {
            var s when Validate(s) => this.Parse(s),
            _ => throw new ArgumentException("srcは0-9の数値または-のみで構成されている必要があります。", nameof(src)),
        };
    }

    public string? this[int i] => i < this.Values.Length ? this.Values[i] : null;

    protected virtual bool Validate(string src) {
        foreach (var c in src) {
            if (Vocabs.Contains(c) is false) {
                return false;
            }
        }

        return true;
    }

    protected virtual ImmutableArray<string> Parse(string source) {
        var blocks = source.Split(this.Separator);

        foreach (var i in 0..this.Vocabs.Length) {
            if (i >= blocks.Length
                || blocks[i].Length != this.Vocabs[i]
                || this.CheckVocabContains(blocks[i]) is false) {
                throw new Exception();
            }
        }

        return blocks.ToImmutableArray();
    }

    bool CheckVocabContains(string text) {
        foreach (var c in text) {
            if (this.Vocabs.Contains(c) is false) {
                return false;
            }
        }

        return true;
    }

    public override string ToString() {
        return this.Values.JoinStrings(this.Separator);
    }
}