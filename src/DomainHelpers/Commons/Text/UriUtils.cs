namespace DomainHelpers.Commons.Text; 
public static class UriUtils {
    /// <summary>
    /// 対象のUriが子ディレクトリかどうかを判定します.
    /// </summary>
    /// <remarks>
    ///
    /// target : http://host.com/first/second/index.html    /// 
    /// parent =
    ///   true   : first
    ///   false  : /first/second/third/
    ///   false  : /first/second/third
    ///   true   : first/second/
    ///   true   : /first/second/
    ///   true   : /first/second/index.html/
    ///   true   : /first/second/index.html
    ///
    /// 先頭と末尾の"/"がない場合は自動的に補間されます.
    /// </remarks>
    /// <param name="url">対象のUri.</param>
    /// <param name="parent">対象のパス.</param>
    /// <returns>判定結果.</returns>
    public static bool IsChild(this Uri url, string parent) {
        try {
            var relative = Path.GetRelativePath(Path.Combine("/", parent), url.AbsolutePath);
            return relative is not (null or "")
                && relative.StartsWith("..") is false
                && Path.IsPathRooted(relative) is false;
        }
        catch {
            return false;
        }
    }

    /// <summary>
    /// 対象のUriが子ディレクトリかどうかを判定します。.
    /// </summary>
    /// <remarks>
    ///
    /// target : http://host.com/first/second/index.html
    ///
    /// true   : first
    /// false  : /first/second/third/
    /// false  : /first/second/third
    /// true   : first/second/
    /// true   : /first/second/
    /// true   : /first/second/index.html/
    /// true   : /first/second/index.html
    ///
    /// 先頭と末尾の"/"がない場合は自動的に補間されます.
    /// </remarks>
    /// <param name="url">対象のUri.</param>
    /// <param name="parent">対象のパス.</param>
    /// <returns>判定結果.</returns>
    public static bool IsChild(this Uri url, Uri parent) {
        var path = parent.IsAbsoluteUri ? parent.LocalPath : parent.OriginalString;
        return url.IsChild(path);
    }

    /// <summary>
    /// 対象のUriが子ディレクトリかどうかを判定します。.
    /// </summary>
    /// <remarks>
    ///
    /// target : first/second/index.html
    ///
    /// true   : first
    /// false  : /first/second/third/
    /// false  : /first/second/third
    /// true   : first/second/
    /// true   : /first/second/
    /// true   : /first/second/index.html/
    /// true   : /first/second/index.html
    ///
    /// 先頭と末尾の"/"がない場合は自動的に補間されます.
    /// </remarks>
    /// <param name="url">対象のUri.</param>
    /// <param name="parent">対象のパス.</param>
    /// <returns>判定結果.</returns>
    public static bool IsChild(string parent, string path) {
        try {
            var relative = Path.GetRelativePath(Path.Combine("/", parent), Path.Combine("/", path));
            return relative is not (null or "")
                && relative.StartsWith("..") is false
                && Path.IsPathRooted(relative) is false;
        }
        catch {
            return false;
        }
    }
}