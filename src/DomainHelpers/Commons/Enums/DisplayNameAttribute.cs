namespace DomainHelpers.Commons.Enums; 
public class DisplayNameAttribute : Attribute {
    public DisplayNameAttribute(string displayName) {
        DisplayName = displayName;
    }

    public string DisplayName { get; init; }
}