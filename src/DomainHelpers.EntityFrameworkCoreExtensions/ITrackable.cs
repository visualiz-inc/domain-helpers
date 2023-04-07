namespace MoriFlocky.Infrastructure.AzureSql;

public interface IDateTimeTrackable {
    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}

public interface ICreatedTrackable {
    public string CreatedBy { get; init; }

    public string UpdatedBy { get; init; }
}