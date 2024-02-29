namespace Dal;
using DalApi;
using System.Xml.Linq;

internal class ClockImplementation : IClock
{
    private readonly string s_clock_xml = "data-config";

    public DateTime? GetCurrentTime()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml).Element("CurrentDate")!;
        if(root.Value == "")
        {
            return null;
        }
        return DateTime.Parse(root.Value);
    }

    public DateTime? GetStartProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml).Element("StartProject")!;
        if(root.Value == "")
        {
            return null;
        }
        return DateTime.Parse(root.Value);
    }

    public void resetTimeLine()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);
        root.Element("StartProject")!.Value = "";
        root.Element("CurrentDate")!.Value = "";
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }

    public void SetCurrentTime(DateTime currentTime)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);
        root.Element("CurrentDate")!.Value = currentTime.ToString();
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }

    public void SetStartProject(DateTime startProject)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);
        root.Element("StartProject")!.Value = startProject.ToString();
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }
}
