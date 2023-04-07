namespace DomainHelpers.Domain {
    namespace MoriFlocky.Domain.Common;

    public interface IDateTimeTrackable {
        public DateTime CreatedAt { get; init; }

        public DateTime UpdatedAt { get; init; }
    }

    public interface IAccountTrackable {
        public AccountMeta CreatedBy { get; init; }

        public AccountMeta UpdatedBy { get; init; }
    }
}