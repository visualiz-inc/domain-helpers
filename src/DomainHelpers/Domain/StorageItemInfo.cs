namespace MoriFlocky.Domain.Common;

public record StorageItemInfo {
    public string Name { get; }

    public Uri Uri { get; }

    public StorageItemInfo(string name, Uri uri) {
        Name = name;
        Uri = uri;
    }
}