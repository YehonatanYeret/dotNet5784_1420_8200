namespace PL;

using BlApi;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

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
internal class ConvertIdToVisible : IValueConverter
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
internal class ConvertIdToCollapsed : IValueConverter
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
//internal class ConvertEngineerToVisible : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        return (BO.Engineer)value is null ? "Visible" : "Collapsed";
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}

/// <summary>
/// convert the effort time to width
/// </summary>
internal class ConvertEffortTimeToWidth : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((TimeSpan)value).Days * 2;
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
        if (value is null)
            return new Thickness(0, 0, 0, 0);
        // TimeSpan day = TimeSpan.FromDays((int)((DateTime)value).);
        return new Thickness((((DateTime)value) - (DateTime)BlApi.Factory.Get().Clock.GetStartProject()!).Days * 2, 1, 0, 1);
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
/// to bind two buttons visibility
/// </summary>
internal class ConvertHiddenToVissible : IValueConverter
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
        if (((Visibility)value) == Visibility.Visible)
            return "Collapsed";
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

/// <summary>
/// to bind status to color for the gantt chart
/// </summary>
internal class ConvertTaskStatusToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //if the task is in delay
        if ((BlApi.Factory.Get().Task.InDelay((int)value)))
            return "#cc3232";

        //if the task is done
        if ((BlApi.Factory.Get().Task.Read((int)value).Status.Equals(BO.Status.Done)))
            return "#2dc937";

        //if the task is on track
        if ((BlApi.Factory.Get().Task.Read((int)value).Status.Equals(BO.Status.OnTrack)))
            return "#99c140";

        //if the task is scheduled
        return "#baffb9";

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// convert string to picture
/// </summary>
internal class ConvertStringToImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BitmapImage bitmap = new BitmapImage();
        //start init
        bitmap.BeginInit();

        //if the value is null or empty we will show the no image found picture
        if (value == null || string.IsNullOrEmpty((string)value))
            bitmap.UriSource = new Uri("../Images/noImageFound.jpg", UriKind.RelativeOrAbsolute);
        //else we will show the image
        else
            bitmap.StreamSource = new MemoryStream(System.Convert.FromBase64String((string)value));
        bitmap.EndInit();

        return bitmap;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertEngineerAndStatusToVisible : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)values[0] != 0)
            return Visibility.Collapsed;

        return (bool)values[1] ? Visibility.Collapsed : Visibility.Visible;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}