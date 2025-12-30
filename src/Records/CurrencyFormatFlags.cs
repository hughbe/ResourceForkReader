namespace ResourceForkReader.Records;

/// <summary>
/// Flags for currency format in a Numeric Format Record ('INTL' and 'intl0').
/// </summary>
[Flags]
public enum CurrencyFormatFlags : byte
{
    /// <summary>
    /// No flags set.
    /// </summary>
    None = 0,

    /// <summary>
    /// Currency symbol appears after the amount.
    /// </summary>
    CurrencySymbolLeads = 16,

    /// <summary>
    /// Use minus sign for negative values.
    /// </summary>
    UseMinusSignForNegativeValues = 32,

    /// <summary>
    /// Use trailing decimal zeroes.
    /// </summary>
    UseTrailingDecimalZeroes = 64,

    /// <summary>
    /// Use leading integer zero.
    /// </summary>
    UseLeadingIntegerZero = 128,
}
