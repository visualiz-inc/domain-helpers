using DomainHelpers;
using MoriFlocky.Domain.Accounts;

namespace MoriFlocky.Domain.Common; 
[PrefixedUlid("storage_file")]
public partial record StorageFileInfoId;

public record StorageFileInfo {
    public required StorageFileInfoId StorageFileId { get; init; }

    public required string Uri { get; init; }

    public required string ThumbnailUri { get; init; }

    public required string FileName { get; init; }

    public required string FileSize { get; init; }

    public required StorageFileType FileType { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required AccountMeta CreatedBy { get; init; }

    public required DateTime UpdatedAt { get; init; }

    public required AccountMeta UpdatedBy { get; init; }
}