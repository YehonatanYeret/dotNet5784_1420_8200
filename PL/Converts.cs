namespace PL;

using System;
using System.Globalization;
using System.IO;
using System.Windows;
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
    /// <returns>"Add" if the ID is 0, otherwise "Update".</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    /// <summary>
    /// Converts back from button content to engineer ID (Not implemented).
    /// </summary>
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
    /// <returns>True if the ID is 0, otherwise false.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0;
    }

    /// <summary>
    /// Converts back from boolean value to engineer ID (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts an engineer ID to a column index value.
/// </summary>
internal class ConvertIdToColumn : IValueConverter
{
    /// <summary>
    /// Converts the engineer ID to a column index value.
    /// </summary>
    /// <returns>1 if the ID is 0, otherwise 0.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((int)value == 0) ? 1 : 0;
    }

    /// <summary>
    /// Converts back from column index value to engineer ID (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a task ID to a visibility value ("Visible" if ID is not 0, "Collapsed" otherwise).
/// </summary>
internal class ConvertIdToVisible : IValueConverter
{
    /// <summary>
    /// Converts the task ID to a visibility value.
    /// </summary>
    /// <returns>"Visible" if the ID is not 0, otherwise "Collapsed".</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value != 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts back from visibility value to task ID (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a task ID to a visibility value ("Visible" if ID is 0, "Collapsed" otherwise).
/// </summary>
internal class ConvertIdToCollapsed : IValueConverter
{
    /// <summary>
    /// Converts the task ID to a visibility value.
    /// </summary>
    /// <returns>"Visible" if the ID is 0, otherwise "Collapsed".</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts back from visibility value to task ID (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts effort time to width for the Gantt Chart.
/// </summary>
internal class ConvertEffortTimeToWidth : IValueConverter
{
    /// <summary>
    /// Converts effort time to width for the Gantt Chart.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((TimeSpan)value).Days * 2;
    }

    /// <summary>
    /// Converts back from width to effort time (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts start date to margin for positioning.
/// </summary>
internal class ConvertStartDateToMargin : IValueConverter
{
    /// <summary>
    /// Converts start date to margin for positioning.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return new Thickness(0, 0, 0, 0);

        return new Thickness((((DateTime)value) - (DateTime)BlApi.Factory.Get().Clock.GetStartProject()!).Days * 2, 1, 0, 1);
    }

    /// <summary>
    /// Converts back from margin to start date (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts engineer ID to engineer name.
/// </summary>
internal class ConvertEngineerToEngineerName : IValueConverter
{
    /// <summary>
    /// Converts engineer ID to engineer name.
    /// </summary>
    /// <returns>The name of the engineer or "Choose Engineer" if ID is null.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is null ? "Choose Engineer" : BlApi.Factory.Get().Engineer.Read((value as BO.EngineerInTask)!.Id).Name;
    }

    /// <summary>
    /// Converts back from engineer name to engineer ID (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts visibility value to its opposite.
/// </summary>
internal class ConvertHiddenToVissible : IValueConverter
{
    /// <summary>
    /// Converts visibility value to its opposite.
    /// </summary>
    /// <returns>"Collapsed" if the input is "Visible", and vice versa.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (((Visibility)value) == Visibility.Visible)
            return Visibility.Collapsed;

        return Visibility.Visible;
    }

    /// <summary>
    /// Converts back from visibility value to its opposite (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts task status to color for the Gantt Chart.
/// </summary>
internal class ConvertTaskStatusToColor : IValueConverter
{
    /// <summary>
    /// Converts task status to color for the Gantt Chart.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // If the task is in delay
        if ((BlApi.Factory.Get().Task.InDelay((int)value)))
            return "#cc3232";

        // Get the status of the task
        BO.Status status = BlApi.Factory.Get().Task.Read((int)value).Status;

        // If the task is done
        if (status.Equals(BO.Status.Done))
            return "#2dc937";

        // If the task is on track
        if (status.Equals(BO.Status.OnTrack))
            return "#99c140";

        // If the task is scheduled
        return "#baffb9";
    }

    /// <summary>
    /// Converts back from color to task status (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a base64-encoded string to a BitmapImage.
/// </summary>
internal class ConvertStringToImage : IValueConverter
{
    /// <summary>
    /// Converts a base64-encoded string to a BitmapImage.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BitmapImage bitmap = new BitmapImage();
        // Start initialization
        bitmap.BeginInit();

        // If the value is null or empty, show the no image found picture with the relative path
        if (value == null || string.IsNullOrEmpty((string)value))
            bitmap.UriSource = new Uri("../Images/noImageFound.jpg", UriKind.RelativeOrAbsolute);
        // Else, show the image after decoding the base64 string
        else
            bitmap.StreamSource = new MemoryStream(System.Convert.FromBase64String((string)value));

        // End initialization
        bitmap.EndInit();

        return bitmap;
    }

    /// <summary>
    /// Converts back from BitmapImage to base64-encoded string (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a set of values to visibility based on conditions.
/// </summary>
internal class ConvertEngineerAndStatusToVisible : IMultiValueConverter
{
    /// <summary>
    /// Converts a set of values to visibility based on conditions.
    /// </summary>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)values[0] != 0)
            return Visibility.Collapsed;

        // If there are 2 value, return the value based on the value 1
        if (values.Length == 2)
            return ((bool)values[1]) ? Visibility.Collapsed : Visibility.Visible;

        // If the project is started or we are in recovery mode
        return ((bool)values[1] || (int)values[2] == 0) ? Visibility.Collapsed : Visibility.Visible;
    }

    /// <summary>
    /// Converts back from visibility to a set of values (Not implemented).
    /// </summary>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a set of values to visibility based on conditions.
/// </summary>
internal class ConvertTaskndStatusToVisible : IMultiValueConverter
{
    /// <summary>
    /// Converts a set of values to visibility based on conditions.
    /// </summary>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // If we are in create mode or the project is started hide the button
        if ((int)values[0] == 0 || !(bool)values[1])
            return Visibility.Collapsed;

        // If the project isn't started or we are in update mode
        return Visibility.Visible;
    }

    /// <summary>
    /// Converts back from visibility to a set of values (Not implemented).
    /// </summary>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Convert visibility to column index
/// </summary>
internal class ConvertVisibilityToColumn : IValueConverter
{
    /// <summary>
    ///  Convert visibility to column index
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if((Visibility)value == Visibility.Visible)
            return 0;

        return 1;
    }

    /// <summary>
    /// Converts back from visibility to num of column (Not implemented).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
