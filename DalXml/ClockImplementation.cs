namespace Dal;
using DalApi;
using System.Xml.Linq;

internal class ClockImplementation : IClock
{
    private readonly string s_clock_xml = "data-config";

    public DateTime? GetEndProject()
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml).Element("EndProject")!;
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
        root.Element("EndProject")!.Value = "";
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
    }

    public DateTime? SetEndProject(DateTime endProject)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);
        root.Element("EndProject")!.Value = endProject.ToString();
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
        return endProject;
    }

    public DateTime? SetStartProject(DateTime startProject)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_clock_xml);
        root.Element("StartProject")!.Value = startProject.ToString();
        XMLTools.SaveListToXMLElement(root, s_clock_xml);
        return startProject;
    }
}
