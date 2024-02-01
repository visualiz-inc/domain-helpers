using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DomainHelpers.Commons.Primitives;

/// <summary>
/// <para>An amount of digital data. Create instances using the constructor or struct declarations.</para>
/// <para><c>var kilobyte = new DataSize(1024);</c></para>
/// <para><c>var kilobyte = new DataSize(1, DataUnit.Kilobyte);</c></para>
/// </summary>
[Serializable]
public struct DataSize : IComparable<DataSize> {

    public double Quantity;
    public DataUnit Unit;

    private double AsBits => Quantity * CountBitsInUnit(Unit);

    public long AsByte => ConvertToUnit(DataUnit.Byte);

    /// <summary>
    /// <para>Create a new instance with the given quantity of the given unit of data.</para>
    /// <para><c>var kilobyte = new DataSize(1, DataUnit.Kilobyte);</c></para>
    /// </summary>
    /// <param name="quantity">How much of the given data <c>unit</c> to represent.</param>
    /// <param name="unit">The unit of measure of the given <c>quantity</c> of data.</param>
    public DataSize(double quantity, DataUnit unit) {
        Quantity = quantity;
        Unit = unit;
    }

    /// <summary>
    /// <para>Create a new instance with the given quantity of bytes.</para>
    /// <para><c>var fileSize = new DataSize(new FileInfo(fileName).Length);</c></para>
    /// </summary>
    /// <param name="bytes">How many bytes to represent.</param>
    public DataSize(long bytes) : this(bytes, DataUnit.Byte) { }

    /// <summary>
    /// <para>Convert the data size to the automatically-chosen best fit unit. This will be the largest unit that represents
    /// the data size as a number greater than or equal to one.</para>
    /// <para><c>new DataSize(1024).Normalize().ToString();</c> → <c>1.00 KB</c></para>
    /// </summary>
    /// <param name="useBitsInsteadOfBytes"><c>true</c> to choose a multiple of bits (bits, kilobits, megabits, etc.), or <c>false</c> (the default) to choose a multiple of bytes (bytes, kilobytes, megabytes, etc.).</param>
    /// <returns>A new instance with the normalized quantity and unit. The original instance is unchanged.</returns>
    public DataSize Normalize(bool useBitsInsteadOfBytes = false) {
        var inputBytes = ConvertToUnit(DataUnit.Byte).Quantity;
        var orderOfMagnitude = (int)Math.Max(0, Math.Floor(Math.Log(Math.Abs(inputBytes), useBitsInsteadOfBytes ? 1000 : 1024)));
        var outputUnit = ForMagnitude(orderOfMagnitude, useBitsInsteadOfBytes);
        return ConvertToUnit(outputUnit);
    }

    /// <summary>
    /// <para>Convert the data size to the given unit.</para>
    /// <para><c>new DataSize(1024).ConvertToUnit(DataUnit.Kilobyte).ToString();</c> → <c>1.00 KB</c></para>
    /// </summary>
    /// <param name="destinationUnit">The data size unit that the resulting instance should use.</param>
    /// <returns>A new instance with the converted quantity and unit. The original instance is unchanged.</returns>
    public DataSize ConvertToUnit(DataUnit destinationUnit) {
        return new DataSize(AsBits / CountBitsInUnit(destinationUnit), destinationUnit);
    }

    /// <summary>
    /// <para>Get a data size unit from its string name or abbreviation.</para>
    /// <para>Supports units of bits and bytes, including the SI units like kibibytes, as well as all their abbreviations.</para>
    /// <para>Some abbreviations are case-insensitive, such as <c>megabyte</c>, but others are case-sensitive, like <c>mb</c> and <c>MB</c> because one means megabits and the other means megabytes.</para>
    /// <para>For example, all the inputs that will be parsed as <c>DataUnit.Megabyte</c> are <c>M</c>, <c>MB</c>, <c>megabyte</c>, <c>mbyte</c>, <c>mib</c>, and <c>mebibyte</c> (the first two are case-sensitive).</para>
    /// <para>Usage: <c>DataUnit megabyte = DataSize.ParseUnit("megabyte");</c></para>
    /// </summary>
    /// <param name="unitNameOrAbbreviation">The name (e.g. <c>kilobyte</c>) or abbreviation (e.g. <c>kB</c>) of a data size unit.</param>
    /// <returns>The <see cref="Unit"/> value that represents the matched data size unit.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The given name does not match any known units or their abbreviations.</exception>
    public static DataUnit ParseUnit(string unitNameOrAbbreviation) {
        return unitNameOrAbbreviation.ToLowerInvariant() switch {
            "byte" => DataUnit.Byte,
            "kilobyte" or "kbyte" or "kib" or "kibibyte" => DataUnit.Kilobyte,
            "megabyte" or "mbyte" or "mib" or "mebibyte" => DataUnit.Megabyte,
            "gigabyte" or "gbyte" or "gib" or "gibibyte" => DataUnit.Gigabyte,
            "terabyte" or "tbyte" or "tib" or "tebibyte" => DataUnit.Terabyte,
            "petabyte" or "pbyte" or "pib" or "pebibyte" => DataUnit.Petabyte,
            "exabyte" or "ebyte" or "eib" or "exbibyte" => DataUnit.Exabyte,
            "bit" => DataUnit.Bit,
            "kilobit" or "kbit" => DataUnit.Kilobit,
            "megabit" or "mbit" => DataUnit.Megabit,
            "gigabit" or "gbit" => DataUnit.Gigabit,
            "terabit" or "tbit" => DataUnit.Terabit,
            "petabit" or "pbit" => DataUnit.Petabit,
            "exabit" or "ebit" => DataUnit.Exabit,
            _ => unitNameOrAbbreviation switch {
                "B" => DataUnit.Byte,
                "kB" or "KB" or "K" => DataUnit.Kilobyte,
                "MB" or "M" => DataUnit.Megabyte,
                "GB" or "G" => DataUnit.Gigabyte,
                "TB" or "T" => DataUnit.Terabyte,
                "PB" or "P" => DataUnit.Petabyte,
                "EB" or "E" => DataUnit.Exabyte,
                "b" => DataUnit.Bit,
                "kb" or "Kb" or "k" => DataUnit.Kilobit,
                "mb" or "Mb" or "m" => DataUnit.Megabit,
                "Gb" or "gb" or "g" => DataUnit.Gigabit,
                "Tb" or "tb" or "t" => DataUnit.Terabit,
                "Pb" or "pb" or "p" => DataUnit.Petabit,
                "Eb" or "eb" or "e" => DataUnit.Exabit,
                _ => throw new ArgumentOutOfRangeException("Unrecognized abbreviation for data size unit " + unitNameOrAbbreviation),
            },
        };
    }

    private static ulong CountBitsInUnit(DataUnit sourceUnit) {
        return sourceUnit switch {
            DataUnit.Byte => 8,
            DataUnit.Kilobyte => (ulong)8 << 10,
            DataUnit.Megabyte => (ulong)8 << 20,
            DataUnit.Gigabyte => (ulong)8 << 30,
            DataUnit.Terabyte => (ulong)8 << 40,
            DataUnit.Petabyte => (ulong)8 << 50,
            DataUnit.Exabyte => (ulong)8 << 60,
            DataUnit.Bit => 1,
            DataUnit.Kilobit => 1000L,
            DataUnit.Megabit => 1000L * 1000,
            DataUnit.Gigabit => 1000L * 1000 * 1000,
            DataUnit.Terabit => 1000L * 1000 * 1000 * 1000,
            DataUnit.Petabit => 1000L * 1000 * 1000 * 1000 * 1000,
            DataUnit.Exabit => 1000L * 1000 * 1000 * 1000 * 1000 * 1000,
            _ => throw new ArgumentOutOfRangeException(nameof(sourceUnit), sourceUnit, null),
        };
    }

    private static DataUnit ForMagnitude(int orderOfMagnitude, bool useBitsInsteadOfBytes) {
        return orderOfMagnitude switch {
            0 => useBitsInsteadOfBytes ? DataUnit.Bit : DataUnit.Byte,
            1 => useBitsInsteadOfBytes ? DataUnit.Kilobit : DataUnit.Kilobyte,
            2 => useBitsInsteadOfBytes ? DataUnit.Megabit : DataUnit.Megabyte,
            3 => useBitsInsteadOfBytes ? DataUnit.Gigabit : DataUnit.Gigabyte,
            4 => useBitsInsteadOfBytes ? DataUnit.Terabit : DataUnit.Terabyte,
            5 => useBitsInsteadOfBytes ? DataUnit.Petabit : DataUnit.Petabyte,
            _ => useBitsInsteadOfBytes ? DataUnit.Exabit : DataUnit.Exabyte,
        };
    }

    /// <summary>
    /// <para>Format as a string. The quantity is formatted as a number using the current culture's numeric formatting information,
    /// such as thousands separators and precision. The unit's short abbreviation is appended after a space.</para>
    /// <para><c>new DataSize(1536).ConvertToUnit(DataUnit.Kilobyte).ToString();</c> → <c>1.50 KB</c></para>
    /// </summary>
    /// <returns>String with the formatted data quantity and unit abbreviation, separated by a space.</returns>
    public override string ToString() {
        return $"{Quantity:N} {Unit.ToAbbreviation()}";
    }

    /// <summary>
    /// <para>Format as a string. The quantity is formatted as a number using the current culture's numeric formatting information,
    /// such as thousands separators. The number of digits after the decimal place is specified as the <c>precision</c> parameter,
    /// overriding the culture's default numeric precision.</para>
    /// </summary>
    /// <param name="precision">Number of digits after the decimal place to use when formatting the quantity as a number. The
    /// default for en-US is 2. To use the default for the current culture, pass the value <c>-1</c>, or call
    /// <see cref="ToString()"/>.</param>
    /// <param name="normalize"><c>true</c> to first normalize this instance to an automatically-chosen unit before converting it
    /// to a string, or <c>false</c> (the default) to use the original unit this instance was defined with.</param>
    /// <returns>String with the formatted data quantity and unit abbreviation, separated by a space.</returns>
    public string ToString(int precision, bool normalize = false) {
        if (normalize) {
            return Normalize(Unit.IsMultipleOfBits()).ToString(precision);
        }
        else {
            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            if (precision >= 0) {
                culture.NumberFormat.NumberDecimalDigits = precision;
            }

            return Quantity.ToString("N", culture) + " " + Unit.ToAbbreviation();
        }
    }

    public string ToString(int precision, DataUnit unit) {
        return ConvertToUnit(unit).ToString(precision);
    }

    public bool Equals(DataSize other) {
        return AsBits.Equals(other.AsBits);
    }

    public override bool Equals(object? obj) {
        return obj is DataSize other && Equals(other);
    }

    public override int GetHashCode() {
        return AsBits.GetHashCode();
    }

    public int CompareTo(DataSize other) {
        return AsBits.CompareTo(other.AsBits);
    }

    public static bool operator <(DataSize a, DataSize b) => a.AsBits < b.AsBits;

    public static bool operator >(DataSize a, DataSize b) => a.AsBits > b.AsBits;

    public static bool operator <=(DataSize a, DataSize b) => a.AsBits <= b.AsBits;

    public static bool operator >=(DataSize a, DataSize b) => a.AsBits >= b.AsBits;

    public static DataSize operator +(DataSize a, DataSize b) {
        return new DataSize(a.Quantity + b.ConvertToUnit(a.Unit).Quantity, a.Unit);
    }

    public static DataSize operator -(DataSize a, DataSize b) {
        return new DataSize(a.Quantity - b.ConvertToUnit(a.Unit).Quantity, a.Unit);
    }

    public static DataSize operator *(DataSize a, double b) {
        return new DataSize(a.Quantity * b, a.Unit);
    }

    public static DataSize operator /(DataSize a, double b) {
        if (!b.Equals(0)) {
            return new DataSize(a.Quantity / b, a.Unit);
        }
        else {
            throw new DivideByZeroException();
        }
    }

    public static double operator /(DataSize a, DataSize b) {
        if (!b.Quantity.Equals(0)) {
            return a.AsBits / b.AsBits;
        }
        else {
            throw new DivideByZeroException();
        }
    }

    public static bool operator ==(DataSize a, DataSize b) => a.Equals(b);

    public static bool operator !=(DataSize a, DataSize b) => !a.Equals(b);

    public static implicit operator long(DataSize dataSize) => (long)dataSize.AsBits / 8;

    public static implicit operator DataSize(long bytes) => new(bytes);
}

public static class UnitExtensions {

    /// <summary>Get the short version of this unit's name (1-3 characters), such as <c>MB</c>.</summary>
    /// <param name="unit"></param>
    /// <param name="iec"><c>true</c> to return the IEC abbreviation (KiB, MiB, etc.), or <c>false</c> (the default) to return
    /// the JEDEC abbreviation (KB, MB, etc.)</param>
    /// <returns>The abbreviation for this unit.</returns>
    public static string ToAbbreviation(this DataUnit unit, bool iec = false) {
        return unit switch {
            DataUnit.Byte => "B",
            DataUnit.Kilobyte => iec ? "KiB" : "KB",
            DataUnit.Megabyte => iec ? "MiB" : "MB",
            DataUnit.Gigabyte => iec ? "GiB" : "GB",
            DataUnit.Terabyte => iec ? "TiB" : "TB",
            DataUnit.Petabyte => iec ? "PiB" : "PB",
            DataUnit.Exabyte => iec ? "EiB" : "EB",
            DataUnit.Bit => "b",
            DataUnit.Kilobit => "kb",
            DataUnit.Megabit => "mb",
            DataUnit.Gigabit => "gb",
            DataUnit.Terabit => "tb",
            DataUnit.Petabit => "pb",
            DataUnit.Exabit => "eb",
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null),
        };
    }

    /// <summary>
    /// Get the long version of this unit's name, such as <c>megabyte</c>.
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="iec"><c>true</c> to return the IEC name (kibibyte, mebibyte, etc.), or <c>false</c> (the default) to return
    /// the JEDEC name (kilobyte, megabyte, etc.)</param>
    /// <returns>The name of this unit.</returns>
    public static string ToName(this DataUnit unit, bool iec = false) {
        return unit switch {
            DataUnit.Byte => "byte",
            DataUnit.Kilobyte => iec ? "kibibyte" : "kilobyte",
            DataUnit.Megabyte => iec ? "mebibyte" : "megabyte",
            DataUnit.Gigabyte => iec ? "gibibyte" : "gigabyte",
            DataUnit.Terabyte => iec ? "tebibyte" : "terabyte",
            DataUnit.Petabyte => iec ? "pebibyte" : "petabyte",
            DataUnit.Exabyte => iec ? "exbibyte" : "exabyte",
            DataUnit.Bit => "bit",
            DataUnit.Kilobit => "kilobit",
            DataUnit.Megabit => "megabit",
            DataUnit.Gigabit => "gigabit",
            DataUnit.Terabit => "terabit",
            DataUnit.Petabit => "petabit",
            DataUnit.Exabit => "exabit",
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null),
        };
    }

    public static bool IsMultipleOfBits(this DataUnit unit) {
        return unit switch {
            DataUnit.Byte or DataUnit.Kilobyte or DataUnit.Megabyte or DataUnit.Gigabyte or DataUnit.Terabyte or DataUnit.Petabyte or DataUnit.Exabyte => false,
            DataUnit.Bit or DataUnit.Kilobit or DataUnit.Megabit or DataUnit.Gigabit or DataUnit.Terabit or DataUnit.Petabit or DataUnit.Exabit => true,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null),
        };
    }

    public static DataSize Quantity(this DataUnit unit, double quantity) {
        return new DataSize(quantity, unit);
    }
}

/// <summary>
/// <para>Orders of magnitude of data, from bit and byte to exabit and exabyte.</para>
/// <para>Kilobits and other *bits units are multiples of 1000 of the next smaller unit. For example, a megabit is 1,000,000 bits (1000 * 1000).</para>
/// <para>Kilobytes and other *bytes units are multiples of 1024 of the next smaller unit. For example, a megabyte is 1,048,576 bytes (1024 * 1024).</para>
/// </summary>
public enum DataUnit : long {
    /// <summary>
    /// 1 bit
    /// </summary>
    Bit = (1 << 0) * 8L,

    /// <summary>
    /// 8 bits
    /// </summary>
    Byte = 1 << 0,

    /// <summary>
    /// 1000 bits
    /// </summary>
    Kilobit = (1 << 10) / 8L,

    /// <summary>
    /// 1024 bytes
    /// </summary>
    Kilobyte = 1 << 10,

    /// <summary>
    /// 1000 kilobits, or 1,000,000 bits
    /// </summary>
    Megabit = (1 << 20) * 8L,

    /// <summary>
    /// 1024 kilobytes, or 1,048,576 bytes
    /// </summary>
    Megabyte = 1 << 20,

    /// <summary>
    /// 1000 megabits, or 1,000,000,000 bits
    /// </summary>
    Gigabit = (1 << 30) * 8L,

    /// <summary>
    /// 1024 megabytes, or 1,073,741,824 bytes
    /// </summary>
    Gigabyte = 1 << 30,

    /// <summary>
    /// 1000 gigabits, or 1,000,000,000,000 bits
    /// </summary>
    Terabit = (1 << 40) * 8L,

    /// <summary>
    /// 1024 gigabytes, or 1,099,511,627,776 bytes
    /// </summary>
    Terabyte = 1 << 40,

    /// <summary>
    /// 1000 terabits, or 1,000,000,000,000,000 bits
    /// </summary>
    Petabit = (1 << 50) * 8L,

    /// <summary>
    /// 1024 terabytes, or 1,125,899,906,842,624 bytes
    /// </summary>
    Petabyte = 1 << 50,

    /// <summary>
    /// 1000 petabits, or 1,000,000,000,000,000,000 bits
    /// </summary>
    Exabit = (1 << 60) * 8L,

    /// <summary>
    /// 1024 petabytes, or 1,152,921,504,606,846,976 bytes
    /// </summary>
    Exabyte = 1 << 60,
}

public static class IntToUnitExtension {
    public static DataSize Bit<TNum>(this long num) where TNum : INumber<TNum> => new(num, DataUnit.Bit);
    public static DataSize Byte(this long num) => new(num, DataUnit.Byte);
    public static DataSize KBit(this long num) => new(num, DataUnit.Kilobit);
    public static DataSize KB(this long num) => new(num, DataUnit.Kilobyte);
    public static DataSize MBit(this long num) => new(num, DataUnit.Megabit);
    public static DataSize MB(this long num) => new(num, DataUnit.Megabyte);
    public static DataSize GBit(this long num) => new(num, DataUnit.Gigabit);
    public static DataSize GB(this long num) => new(num, DataUnit.Gigabyte);
    public static DataSize TBit(this long num) => new(num, DataUnit.Terabit);
    public static DataSize TB(this long num) => new(num, DataUnit.Terabyte);
    public static DataSize PBit(this long num) => new(num, DataUnit.Petabit);
    public static DataSize PB(this long num) => new(num, DataUnit.Petabyte);
    public static DataSize EBit(this long num) => new(num, DataUnit.Exabit);
    public static DataSize EB(this long num) => new(num, DataUnit.Exabyte);

    public static DataSize Bit<TNum>(this int num) where TNum : INumber<TNum> => new(num, DataUnit.Bit);
    public static DataSize Byte(this int num) => new(num, DataUnit.Byte);
    public static DataSize KBit(this int num) => new(num, DataUnit.Kilobit);
    public static DataSize KB(this int num) => new(num, DataUnit.Kilobyte);
    public static DataSize MBit(this int num) => new(num, DataUnit.Megabit);
    public static DataSize MB(this int num) => new(num, DataUnit.Megabyte);
    public static DataSize GBit(this int num) => new(num, DataUnit.Gigabit);
    public static DataSize GB(this int num) => new(num, DataUnit.Gigabyte);
    public static DataSize TBit(this int num) => new(num, DataUnit.Terabit);
    public static DataSize TB(this int num) => new(num, DataUnit.Terabyte);
    public static DataSize PBit(this int num) => new(num, DataUnit.Petabit);
    public static DataSize PB(this int num) => new(num, DataUnit.Petabyte);
    public static DataSize EBit(this int num) => new(num, DataUnit.Exabit);
    public static DataSize EB(this int num) => new(num, DataUnit.Exabyte);
}

public static class DataUnitConverter {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ToLong(this DataUnit unit) => (long)unit;
}