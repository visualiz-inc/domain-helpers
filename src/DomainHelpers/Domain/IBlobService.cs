namespace MoriFlocky.Domain.Common;

/// <summary>
/// バイナリデータの保存や読み出しをするサービスのメカニズムを提供します。
/// </summary>
public interface IBlobService {
    Task<StorageItemInfo> SaveAsync(string containerName, string name, Stream stream);

    Task<StorageItemInfo> SaveAsync(string containerName, string name, byte[] bin) {
        return SaveAsync(containerName, name, new MemoryStream(bin, false));
    }

    Task<bool> DeleteAsync(string containerName, string name);
}