namespace PL;

using BlApi;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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

/// <summary>
/// Converts an engineer ID to a boolean value (true if ID is 0, false otherwise).
/// </summary>
internal class ConvertIdToColumn : IValueConverter
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
        return ((int)value == 0) ? 1 : 0;
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
/// Converts an task ID to a boolean value (true if ID is 0, false otherwise).
/// </summary>
internal class ConvertEngineerIdToVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value != 0 ? "Visible" : "Collapsed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts an task ID to a boolean value (true if ID is 0, false otherwise).
/// </summary>
internal class ConvertEngineerIdToCollapsed : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Visible" : "Collapsed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts an task ID to a boolean value (true if ID is 0, false otherwise).
/// </summary>
internal class ConvertWindowOwnerToVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (string)value != "EngineerShowWindow" ? "Vissible" : "Collapsed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// convert the effort time to width
/// </summary>
internal class ConvertEffortTimeToWidth : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((TimeSpan)value).TotalDays*2;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// convert the start date to margin
/// </summary>
internal class ConvertStartDateToMargin : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is null)
            return new Thickness(0, 0, 0, 0);
        return new Thickness((((DateTime)value) - (DateTime)BlApi.Factory.Get().Clock.GetStartProject()!).TotalDays , 1, 0, 1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// convert the engineer id to engineer name
/// </summary>
internal class ConvertEngineerToEngineerName : IValueConverter
{
    /// <summary>
    /// Convert the engineer id to engineer name
    /// </summary>
    /// <param name="value">The engineer id</param>
    /// <param name="targetType">The type of the target property (not used)</param>
    /// <param name="parameter">An optional parameter (not used)</param>
    /// <param name="culture">The culture to use in the converter (not used)</param>
    /// <returns>The name of the engineer</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        
        return value is null ? "Choose Engineer" : BlApi.Factory.Get().Engineer.Read((value as BO.EngineerInTask)!.Id).Name;
    }


    /// <summary>
    /// Not implemented for one-way conversion.
    /// </summary>
    /// <param name="value">The engineer id</param>
    /// <param name="targetType">The type of the target property (not used)</param>
    /// <param name="parameter">An optional parameter (not used)</param>
    /// <param name="culture">The culture to use in the converter (not used)</param>
    /// <returns>NotImplementedException is thrown</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// to bind two buttons
/// </summary>
internal class ConverHiddenTOVissible : IValueConverter
{
    /// <summary>
    /// Convert the engineer id to engineer name
    /// </summary>
    /// <param name="value">The engineer id</param>
    /// <param name="targetType">The type of the target property (not used)</param>
    /// <param name="parameter">An optional parameter (not used)</param>
    /// <param name="culture">The culture to use in the converter (not used)</param>
    /// <returns>The name of the engineer</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (((Button)value).Visibility == Visibility.Visible)
            return "Hidden";
        return "Visible";
    }


    /// <summary>
    /// Not implemented for one-way conversion.
    /// </summary>
    /// <param name="value">The engineer id</param>
    /// <param name="targetType">The type of the target property (not used)</param>
    /// <param name="parameter">An optional parameter (not used)</param>
    /// <param name="culture">The culture to use in the converter (not used)</param>
    /// <returns>NotImplementedException is thrown</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

