using System;
using System.ComponentModel.DataAnnotations;

namespace InMemorySyncTest.Model;

public class Mock : IEquatable<Mock>
{
    public Mock(int id)
    {
        Id = id;
    }

    [Key] public int Id { get; set; }


    public bool Equals(Mock? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Mock)obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public static bool operator ==(Mock? left, Mock? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Mock? left, Mock? right)
    {
        return !Equals(left, right);
    }
}