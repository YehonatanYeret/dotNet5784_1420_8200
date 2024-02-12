namespace PL;

using System;
using System.Globalization;
using System.Windows.Data;

/// <summary>
/// Converts an engineer ID to content for button labels (Add or Update).
/// </summary>
internal class ConvertIdToContent : IValueConverter
{
    /// <summary>
    /// Converts the engineer ID to button content.
    /// </summary>
    /// <param name="value">The engineer ID.</param>
    /// <param name="targetType">The type of the target property (not used).</param>
    /// <param name="parameter">An optional parameter (not used).</param>
    /// <param name="culture">The culture to use in the converter (not used).</param>
    /// <returns>"Add" if the ID is 0, otherwise "Update".</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    /// <summary>
    /// Not implemented for one-way conversion.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property (not used).</param>
    /// <param name="parameter">An optional parameter (not used).</param>
    /// <param name="culture">The culture to use in the converter (not used).</param>
    /// <returns>NotImplementedException is thrown.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// Converts an engineer ID to a boolean value (true if ID is 0, false otherwise).
/// </summary>
internal class ConvertIdToBoolean : IValueConverter
{
/// <summary>
/// Converts the engineer ID to a boolean value.
/// </summary>
/// <param name="value">The engineer ID.</param>
/// <param name="targetType">The type of the target property (not used).</param>
/// <param name="parameter">An optional parameter (not used).</param>
/// <param name="culture">The culture to use in the converter (not used).</param>
/// <returns>True if the ID is 0, otherwise false.</returns>
public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
{
    return (int)value == 0;
}

/// <summary>
/// Not implemented for one-way conversion.
/// </summary>
/// <param name="value">The value to convert back.</param>
/// <param name="targetType">The type of the target property (not used).</param>
/// <param name="parameter">An optional parameter (not used).</param>
/// <param name="culture">The culture to use in the converter (not used).</param>
/// <returns>NotImplementedException is thrown.</returns>
public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
{
    throw new NotImplementedException();
}
}

