namespace Dal;

using DalApi;
using DO;
using System.Xml.Linq;

/// <summary>
/// Implementation of the <see cref="IDependency"/> interface for handling Dependency operations.
/// </summary>
internal class DepenencyImplemention : IDependency
{
    // XML file name for storing dependencies
    private readonly string s_dependency_xml = "dependency";

    /// <summary>
    /// Creates a new Dependency and adds it to the XML storage.
    /// </summary>
    /// <param name="item">The Dependency to be created.</param>
    /// <returns>The ID assigned to the created Dependency.</returns>
    public int Create(Dependency item)
    {
        XElement? root = XMLTools.LoadListFromXMLElement(s_dependency_xml);

        // Create XML elements for the new Dependency
        XElement dep = new XElement("dependency");

        XElement id = new XElement("Id", Config.NextDependencyId);
        XElement dependentTask = new XElement("dependentTask", item.DependentTask);
        XElement dependentOnTask = new XElement("dependentOnTask", item.DependentOnTask);

        dep.Add(id, dependentTask, dependentOnTask);

        // Add the new Dependency to the XML
        root.Add(dep);

        // Save the updated XML back to the file
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);

        return int.Parse(id.Value);
    }

    /// <summary>
    /// Reads a Dependency from the XML storage based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Dependency to retrieve.</param>
    /// <returns>The Dependency with the specified ID, or null if not found.</returns>
    public Dependency? Read(int id)
    {
        XElement? element = XMLTools.LoadListFromXMLElement(s_dependency_xml).Elements()
            .FirstOrDefault(dep => (int?)dep.Element("Id") == id);

        return element is null ? null : GetDependency(element);
    }

    /// <summary>
    /// Reads a Dependency from the XML storage based on a custom filter.
    /// </summary>
    /// <param name="filter">The filter function to apply.</param>
    /// <returns>The first Dependency that matches the filter, or null if not found.</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependency_xml).Elements()
            .Select(GetDependency)
            .FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all Dependencies from the XML storage, optionally filtered.
    /// </summary>
    /// <param name="filter">An optional filter function to apply.</param>
    /// <returns>A collection of Dependencies that match the filter, or all Dependencies if no filter is specified.</returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
        var elements = XMLTools.LoadListFromXMLElement(s_dependency_xml).Elements();

        if (filter is null)
            return elements.Select(GetDependency);

        return elements.Select(GetDependency).Where(filter);
    }

    /// <summary>
    /// Updates an existing Dependency in the XML storage.
    /// </summary>
    /// <param name="item">The updated Dependency.</param>
    public void Update(Dependency item)
    {
        XElement root = XMLTools.LoadListFromXMLElement(s_dependency_xml);
        XElement? dependency = (from dep in root.Elements()
                                where (int?)dep.Element("Id") == item.Id
                                select dep).FirstOrDefault();

        // If the Dependency does not exist, throw an exception
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");

        // Update the Dependency's properties in the XML
        if (dependency.Element("dependentTask") != null)
            dependency.Element("dependentTask")!.Value = item.DependentTask.ToString()!;
        else
            dependency.Add(new XElement("dependentTask", item.DependentTask.ToString()!));

        if (dependency.Element("dependentOnTask") != null)
            dependency.Element("dependentOnTask")!.Value = item.DependentOnTask.ToString()!;
        else
            dependency.Add(new XElement("dependentOnTask", item.DependentOnTask.ToString()!));
        // Save the updated XML back to the file
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);
    }

    /// <summary>
    /// Deletes a Dependency from the XML storage based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Dependency to delete.</param>
    public void Delete(int id)
    {
        XElement? root = XMLTools.LoadListFromXMLElement(s_dependency_xml);

        // Find the Dependency element with the specified ID
        XElement? dependency = (from dep in root.Elements()
                                where (int?)dep.Element("Id") == id
                                select dep).FirstOrDefault();

        // If the Dependency does not exist, throw an exception
        if (dependency == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");

        // Remove the Dependency element
        dependency.Remove();

        // Save the updated XML back to the file
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);
    }

    /// <summary>
    /// Deletes all Dependencies.
    /// </summary>
    public void DeleteAll()
    {
        XElement? root = XMLTools.LoadListFromXMLElement(s_dependency_xml);

        IEnumerable<XElement?> dependencys = (from dep in root.Elements()
                                              select dep);

        // Remove all the dependencies
        dependencys.Remove();

        // Save the updated XML back to the file
        XMLTools.SaveListToXMLElement(root, s_dependency_xml);

        // Reset the next ID to the defualt value
        XMLTools.ResetId("data-config", "NextDependencyId");
    }

    // Helper method to convert XElement to Dependency
    Dependency GetDependency(XElement element) => new
    (
    Id: (int)element.Element("Id")!,
    DependentTask: (int)element.Element("dependentTask")!,
    DependentOnTask: (int)element.Element("dependentOnTask")!
    );

}