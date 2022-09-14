namespace DomainHelpers.Text; 

public static class NumberTextExtensions {
    public static string ToLocaleString(this int num) {
        return num.ToString("#,0");
    }

    public static string ToLocaleString(this decimal num) {
        return num.ToString("#,0.#");
    }
}