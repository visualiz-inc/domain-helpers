namespace System;

public class DisplayNameAttribute : Attribute {
    public string DisplayName { get; init; }

    public DisplayNameAttribute(string displayName) {
        this.DisplayName = displayName;
    }
}