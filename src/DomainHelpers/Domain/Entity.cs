using DomainHelpers.Domain.Indentifier;

namespace DomainHelpers.Domain;

public abstract class Entity {
    public virtual ValueTask OnSaveAsync() => ValueTask.CompletedTask;

    public override bool Equals(object? obj) {
        if (obj == null || obj is not Entity)
            return false;

        if (object.ReferenceEquals(this, obj))
            return true;

        return GetType() != obj.GetType();
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right) {
        if (Equals(left, null))
            return Equals(right, null);
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right) {
        return !(left == right);
    }
}

public abstract class Entity<TId>(TId id) : Entity {
    int? _requestedHashCode;

    public virtual TId Id { get; protected set; } = id;

    public bool IsTransient() {
        return this.Id.Equals(default);
    }

    public override bool Equals(object? obj) {
        if (obj == null || obj is not Entity) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        if (this.GetType() != obj.GetType()) {
            return false;
        }

        var item = (Entity<TId>)obj;
        if (item.IsTransient() || this.IsTransient()) {
            return false;
        }
        else {
            return item.Id.Equals(Id);
        }
    }

    public override int GetHashCode() {
        if (!IsTransient()) {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }
        else {
            return base.GetHashCode();
        }
    }
}

