namespace Dal;
using DalApi;
using System.Xml.Linq;

/// <summary>
/// ClockImplementation is a class that implements the IClock interface.
/// </summary>
internal class ClockImplementation : IClock
{
    private readonly string s_clock_xml = "data-config";

    /// <summary>
    /// Returns the current time.
    /// </summary>
    /// <returns>The current time.</returns>
    public DateTime? GetCurrentTime()
    {
        // Load the XML file with XElement.
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml).Element("CurrentDate")!;
        if(root.Value == "")
            return null;
        
        return DateTime.Parse(root.Value);
    }

    /// <summary>
    /// Returns the start project time.
    /// </summary>
    /// <returns>The start project time.</returns>
    public DateTime? GetStartProject()
    {
        // Load the XML file with XElement.
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml).Element("StartProject")!;
        if(root.Value == "")
            return null;
        
        return DateTime.Parse(root.Value);
    }

    /// <summary>
    /// Resets the timeline of the project.
    /// </summary>
    public void resetTimeLine()
    {
        // Load the XML file with XElement.
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);

        // Reset the values.
        root.Element("StartProject")!.Value = "";
        root.Element("CurrentDate")!.Value = "";

        // Save the changes.
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }

    /// <summary>
    /// Sets the current time.
    /// </summary>
    /// <param name="currentTime">The current time.</param>
    public void SetCurrentTime(DateTime currentTime)
    {
        // Load the XML file with XElement.
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);

        // Set the current time.
        root.Element("CurrentDate")!.Value = currentTime.ToString();

        // Save the changes.
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }

    /// <summary>
    /// Sets the start project time.
    /// </summary>
    /// <param name="startProject">The start project time.</param>
    public void SetStartProject(DateTime startProject)
    {
        // Load the XML file with XElement.
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);

        // Set the start project time.
        root.Element("StartProject")!.Value = startProject.ToString();

        // Save the changes.
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }
}
