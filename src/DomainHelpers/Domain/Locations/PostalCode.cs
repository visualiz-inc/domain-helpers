using System.Runtime.CompilerServices;

namespace DomainHelpers.Domain.Locations;
/// <summary>
///  郵便番号を表します
/// </summary>
public struct PostalCode {
    /// <summary>
    ///  郵便番号に含めることができる値。
    /// </summary>
    public const string Vocabs = "0123456789-";

    private char _val0 = '0',
        _val1 = '0',
        _val2 = '0',
        _val3 = '0',
        _val4 = '0',
        _val5 = '0',
        _val6 = '0';

    char Val0 => NullCharToZero(_val0);
    char Val1 => NullCharToZero(_val1);
    char Val2 => NullCharToZero(_val2);
    char Val3 => NullCharToZero(_val3);
    char Val4 => NullCharToZero(_val4);
    char Val5 => NullCharToZero(_val5);
    char Val6 => NullCharToZero(_val6);

    public string Left => new(new[] { Val0, Val1, Val2 });

    public string Right => new(new[] { Val3, Val4, Val5, Val6 });

    /// <summary>
    ///  郵便番号値を表します。
    /// </summary>
    public string Value {
        get => new(new[] { Val0, Val1, Val2, Val3, Val4, Val5, Val6 });
        set {
            var normalized = new PostalCode(value).Value;
            _val0 = normalized[0];
            _val1 = normalized[1];
            _val2 = normalized[2];
            _val3 = normalized[3];
            _val4 = normalized[4];
            _val5 = normalized[5];
            _val6 = normalized[6];
        }
    }

    /// <summary>
    /// <see cref="PostalCode" /> 構造体を初期化して新しいインスタンスを生成します。
    /// </summary>
    /// <param name="code">郵便番号ソース.</param>
    /// <exception cref="ArgumentException">郵便番号の形式が無効な場合にスローされます。</exception>
    public PostalCode(string code) {
        var normalized = code switch {
            ({ Length: 7 or 8 })
                => code.Replace("-", "") switch {
                    var s when Validate(s) => s,
                    _
                        => throw new ArgumentException(
                            "郵便番号は0-9の数値またはハイフンのみで構成されている必要があります。",
                            nameof(code)
                        )
                },
            _ => throw new ArgumentException("郵便番号は000-0000または0000000の形式で指定してください。")
        };

        _val0 = normalized[0];
        _val1 = normalized[1];
        _val2 = normalized[2];
        _val3 = normalized[3];
        _val4 = normalized[4];
        _val5 = normalized[5];
        _val6 = normalized[6];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static char NullCharToZero(char c) => c == 0 ? '0' : c;

    /// <summary>
    ///  入力文字列が数値またはハイフンのみで構成されているかチェックします。
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>
    private static bool Validate(string src) {
        foreach (var c in src) {
            if (Vocabs.Contains(c) is false) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ハイフンを取り除いた値を取得します。
    /// </summary>
    /// <returns>期待する値。</returns>
    private string GetWithHphen() {
        return ToString("N");
    }

    /// <summary>
    /// 文字列に変換します。
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return ToString("D");
    }

    /// <summary>
    /// 文字列に変換します。
    /// </summary>
    /// <remarks>
    /// 引数に書式設定を指定することができます。
    /// D|d -> 桁区切りにハイフン
    /// N|n -> 数値のみ
    /// </remarks>
    /// <param name="format">
    /// 書式設定。
    /// </param>
    /// <returns></returns>
    public string ToString(string format) {
        return format switch {
            "D" or "d" => Left + "-" + Right,
            "N" or "n" => Value,
            _ => throw new ArgumentException("フォーマットの指定が間違っています", nameof(format))
        };
    }

    /// <summary>
    /// 整数値に変換します。
    /// </summary>
    /// <returns></returns>
    public int ToInteger() {
        return Convert.ToInt32(GetWithHphen());
    }

    public static bool TryParse(string text, out PostalCode code) {
        try {
            code = new PostalCode(text);
            return true;
        }
        catch {
            code = default;
            return false;
        }
    }
}