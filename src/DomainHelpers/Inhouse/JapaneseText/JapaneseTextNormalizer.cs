using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Inhouse.JapaneseText;

/// <summary>
/// 半角全角置換用エクステンション.
/// </summary>
public static class StringConvExtensions {
    public static Dictionary<string, string> DictHalfFullKana { get; } = new() {
        { "ｱ", "ア" },
        { "ｲ", "イ" },
        { "ｳ", "ウ" },
        { "ｴ", "エ" },
        { "ｵ", "オ" },
        { "ｶ", "カ" },
        { "ｷ", "キ" },
        { "ｸ", "ク" },
        { "ｹ", "ケ" },
        { "ｺ", "コ" },
        { "ｻ", "サ" },
        { "ｼ", "シ" },
        { "ｽ", "ス" },
        { "ｾ", "セ" },
        { "ｿ", "ソ" },
        { "ﾀ", "タ" },
        { "ﾁ", "チ" },
        { "ﾂ", "ツ" },
        { "ﾃ", "テ" },
        { "ﾄ", "ト" },
        { "ﾅ", "ナ" },
        { "ﾆ", "ニ" },
        { "ﾇ", "ヌ" },
        { "ﾈ", "ネ" },
        { "ﾉ", "ノ" },
        { "ﾊ", "ハ" },
        { "ﾋ", "ヒ" },
        { "ﾌ", "フ" },
        { "ﾍ", "ヘ" },
        { "ﾎ", "ホ" },
        { "ﾏ", "マ" },
        { "ﾐ", "ミ" },
        { "ﾑ", "ム" },
        { "ﾒ", "メ" },
        { "ﾓ", "モ" },
        { "ﾔ", "ヤ" },
        { "ﾕ", "ユ" },
        { "ﾖ", "ヨ" },
        { "ﾗ", "ラ" },
        { "ﾘ", "リ" },
        { "ﾙ", "ル" },
        { "ﾚ", "レ" },
        { "ﾛ", "ロ" },
        { "ﾜ", "ワ" },
        { "ｦ", "ヲ" },
        { "ﾝ", "ン" },
        { "ｳﾞ", "ヴ" },
        { "ｶﾞ", "ガ" },
        { "ｷﾞ", "ギ" },
        { "ｸﾞ", "グ" },
        { "ｹﾞ", "ゲ" },
        { "ｺﾞ", "ゴ" },
        { "ｻﾞ", "ザ" },
        { "ｼﾞ", "ジ" },
        { "ｽﾞ", "ズ" },
        { "ｾﾞ", "ゼ" },
        { "ｿﾞ", "ゾ" },
        { "ﾀﾞ", "ダ" },
        { "ﾁﾞ", "ヂ" },
        { "ﾂﾞ", "ヅ" },
        { "ﾃﾞ", "デ" },
        { "ﾄﾞ", "ド" },
        { "ﾊﾞ", "バ" },
        { "ﾋﾞ", "ビ" },
        { "ﾌﾞ", "ブ" },
        { "ﾍﾞ", "ベ" },
        { "ﾎﾞ", "ボ" },
        { "ﾊﾟ", "パ" },
        { "ﾋﾟ", "ピ" },
        { "ﾌﾟ", "プ" },
        { "ﾍﾟ", "ペ" },
        { "ﾎﾟ", "ポ" },
        { "ｧ", "ァ" },
        { "ｨ", "ィ" },
        { "ｩ", "ゥ" },
        { "ｪ", "ェ" },
        { "ｫ", "ォ" },
        { "ｯ", "ッ" },
        { "ｬ", "ャ" },
        { "ｭ", "ュ" },
        { "ｮ", "ョ" },
    };

    public static Dictionary<string, string> DictHalfFullAlphabet { get; } = new() {
        { "a", "ａ" },
        { "b", "ｂ" },
        { "c", "ｃ" },
        { "d", "ｄ" },
        { "e", "ｅ" },
        { "f", "ｆ" },
        { "g", "ｇ" },
        { "h", "ｈ" },
        { "i", "ｉ" },
        { "j", "ｊ" },
        { "k", "ｋ" },
        { "l", "ｌ" },
        { "m", "ｍ" },
        { "n", "ｎ" },
        { "o", "ｏ" },
        { "p", "ｐ" },
        { "q", "ｑ" },
        { "r", "ｒ" },
        { "s", "ｓ" },
        { "t", "ｔ" },
        { "u", "ｕ" },
        { "v", "ｖ" },
        { "w", "ｗ" },
        { "x", "ｘ" },
        { "y", "ｙ" },
        { "z", "ｚ" },
        { "A", "Ａ" },
        { "B", "Ｂ" },
        { "C", "Ｃ" },
        { "D", "Ｄ" },
        { "E", "Ｅ" },
        { "F", "Ｆ" },
        { "G", "Ｇ" },
        { "H", "Ｈ" },
        { "I", "Ｉ" },
        { "J", "Ｊ" },
        { "K", "Ｋ" },
        { "L", "Ｌ" },
        { "M", "Ｍ" },
        { "N", "Ｎ" },
        { "O", "Ｏ" },
        { "P", "Ｐ" },
        { "Q", "Ｑ" },
        { "R", "Ｒ" },
        { "S", "Ｓ" },
        { "T", "Ｔ" },
        { "U", "Ｕ" },
        { "V", "Ｖ" },
        { "W", "Ｗ" },
        { "X", "Ｘ" },
        { "Y", "Ｙ" },
        { "Z", "Ｚ" },
    };

    // 数字と記号（漏れ防止のためUTF16 全角文字コード順に記載）
    public static Dictionary<string, string> DictHalfFullNum { get; } = new() {
        { "!", "！" },
        { "\"", "＂" },
        { "#", "＃" },
        { "$", "＄" },
        { "%", "％" },
        { "&", "＆" },
        { "'", "＇" },
        { "(", "（" },
        { ")", "）" },
        { "*", "＊" },
        { "+", "＋" },
        { ",", "，" },
        { "-", "－" },
        { ".", "．" },
        { "/", "／" },
        { "0", "０" },
        { "1", "１" },
        { "2", "２" },
        { "3", "３" },
        { "4", "４" },
        { "5", "５" },
        { "6", "６" },
        { "7", "７" },
        { "8", "８" },
        { "9", "９" },
        { "．", "｡" },
        { "･", "・" },
        { "ｰ", "ー" },
        { " ", "　" },
        { ":", "：" },
        { ";", "；" },
        { "<", "＜" },
        { "=", "＝" },
        { ">", "＞" },
        { "?", "？" },
        { "@", "＠" },
        { "[", "［" },
        /* { "\\", "＼" },  // 置き換えしないまたは「\」に置き換え。VBは置き換えていない模様 */
        { "]", "］" },
        { "^", "＾" },
        { "_", "＿" },
        { "`", "｀" },
        { "{", "｛" },
        { "|", "｜" },
        { "}", "｝" },
        { "~", "～" },
        { "｡", "。" },
        { "｢", "「" },
        { "｣", "」" },
        { "､", "、" },
        /*
         * { "･", "・" },    // 重複
         * { "ｰ", "ー" },    // 重複
        */
        { "ﾞ", "゛" },
        { "ﾟ", "゜" },
    };

    private static Dictionary<char, string> ReplacerWideToNarrow { get; }

    private static Dictionary<string, string> ReplacerNarrowToWide { get; }

    static StringConvExtensions() {
        ReplacerWideToNarrow = new Dictionary<char, string>();
        ReplacerNarrowToWide = new Dictionary<string, string>();

        foreach (var item in DictHalfFullNum) {
            ReplacerNarrowToWide.Add(item.Key, item.Value);
            ReplacerWideToNarrow.Add(item.Value.ToCharArray().First(), item.Key);
        }

        foreach (var item in DictHalfFullAlphabet) {
            ReplacerNarrowToWide.Add(item.Key, item.Value);
            ReplacerWideToNarrow.Add(item.Value.ToCharArray().First(), item.Key);
        }

        foreach (var item in DictHalfFullKana) {
            ReplacerNarrowToWide.Add(item.Key, item.Value);
            ReplacerWideToNarrow.Add(item.Value.ToCharArray().First(), item.Key);
        }
    }

    /// <summary>
    /// 全角を半角に相互置換します.
    /// </summary>
    /// <param name="text">置換対象のテキスト.</param>
    /// <returns>置換済みのテキスト.</returns>
    public static string StrConvWideToNarrow(this string text) {
        var map = ReplacerWideToNarrow;
        var builder = new StringBuilder();

        foreach (var c in text) {
            if (map.ContainsKey(c)) {
                builder.Append(map[c]);
            }
            else {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// 半角を全角に相互置換します
    /// NOTE:このソースには、文頭に濁点、 半濁点が来ると変換漏れが発生する既知のバグがあります。
    /// </summary>
    /// <param name="text">置換対象のテキスト.</param>
    /// <returns>置換済みのテキスト.</returns>
    public static string StrConvNarrowToWide(this string text) {
        var replacerNarrowToWide = ReplacerNarrowToWide;
        var builder = new StringBuilder();

        string? append = null;
        for (var i = 0; i < text.Length; i++) {
            var c = text[i].ToString();

            // 濁点もしくは半濁点だったら
            if (text[i] is 'ﾞ' or 'ﾟ') {
                if (TryConvertToWide(i, out var text2)) {
                    append = text2;
                }
                else {
                    // 濁点または半濁点じゃない -> 置き換えに成功していたら次の文字はスキップ
                    AppendIfNotNull(append);
                }
            }
            else if (replacerNarrowToWide.ContainsKey(c)) {
                AppendIfNotNull(append);
                append = replacerNarrowToWide[c];
            }
            else {
                AppendIfNotNull(append);

                append = c;
            }
        }
        AppendIfNotNull(append);

        return builder.ToString();

        void AppendIfNotNull(string? text) {
            if (text is not null) {
                builder.Append(text);
            }
        }

        bool TryConvertToWide(int index, out string character) {
            if (index <= 0) {
                character = text[index].ToString();
                return false;
            }

            var c = new string([text[index - 1], text[index],]);
            if (replacerNarrowToWide.TryGetValue(c, out var t)) {
                character = t;
                return true;
            }

            character = replacerNarrowToWide[text[index].ToString()];
            return false;
        }
    }

    /// <summary>
    /// 半角カナを全角に置換します
    /// NOTE:このソースには、文頭に濁点、 半濁点が来ると変換漏れが発生する既知のバグがあります。.
    /// </summary>
    /// <param name="text">置換対象のテキスト.</param>
    /// <returns>置換済みのテキスト.</returns>
    public static string ConvertNarrowKanaToWideKana(this string text) {
        var replacerNarrowToWide = DictHalfFullKana;
        var builder = new StringBuilder();

        (bool IsReplaced, string Text) JapaneseDakuten(int position) {
            if (position <= 0) {
                return (false, text[position].ToString());
            }

            var c = new string([text[position - 1], text[position],]);
            if (replacerNarrowToWide.ContainsKey(c)) {
                return (true, replacerNarrowToWide[c]);
            }

            return (false, replacerNarrowToWide[text[position].ToString()]);
        }

        string? append = null;
        for (var i = 0; i < text.Length; i++) {
            var c = text[i].ToString();

            // 濁点もしくは半濁点だったら
            if (text[i] is 'ﾞ' or 'ﾟ') {
                var (isReplaced, text2) = JapaneseDakuten(i);

                // 濁点または半濁点じゃない -> 置き換えに成功していたら次の文字はスキップ
                if (isReplaced is false) {
                    if (append is not null) {
                        builder.Append(append);
                    }
                }

                append = text2;
            }
            else if (replacerNarrowToWide.ContainsKey(c)) {
                if (append is not null) {
                    builder.Append(append);
                }

                append = replacerNarrowToWide[c];
            }
            else {
                if (append is not null) {
                    builder.Append(append);
                }

                append = c;
            }
        }

        if (append is not null) {
            builder.Append(append);
        }

        return builder.ToString();
    }

    /// <summary>
    /// 全角アルファベットを半角に置換します.
    /// </summary>
    /// <param name="text">置換対象のテキスト.</param>
    /// <returns>置換済みのテキスト.</returns>
    public static string ConvertWideAlphabetToNarrowAlphabet(this string text) {
        var map = DictHalfFullAlphabet.ToDictionary(x => x.Value, x => x.Key);
        var builder = new StringBuilder();

        foreach (var c in text) {
            var t = c.ToString();
            if (map.ContainsKey(t)) {
                builder.Append(map[t]);
            }
            else {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// 全角半角を半角に置換します.
    /// </summary>
    /// <param name="text">置換対象のテキスト.</param>
    /// <returns>置換済みのテキスト.</returns>
    public static string ConvertWideSymbolToNarrowSymbol(this string text) {
        var map = DictHalfFullNum;
        var builder = new StringBuilder();

        foreach (var c in text) {
            var t = c.ToString();
            if (map.ContainsKey(t)) {
                builder.Append(map[t]);
            }
            else {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}