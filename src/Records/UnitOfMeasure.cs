namespace ResourceForkReader.Records;

/// <summary>
/// Units of Measure for the International Resource ('INTL' and 'intl0') Numeric Format Record.
/// </summary>
public enum UnitOfMeasure : byte
{
    /// <summary>
    /// Imperial units (inches, feet, etc.)
    /// </summary>
    Imperial = 0,

    /// <summary>
    /// Metric units (centimeters, meters, etc.)
    /// </summary>
    Metric = 255
}
