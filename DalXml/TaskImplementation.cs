namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implementation of the <see cref="ITask"/> interface for handling Task operations.
/// </summary>
internal class TaskImplementation : ITask
{
    // XML file name for storing tasks
    private readonly string s_task_xml = "tasks";

    /// <summary>
    /// Creates a new Task and adds it to the XML storage.
    /// </summary>
    /// <param name="item">The Task to be created.</param>
    /// <returns>The ID assigned to the created Task.</returns>
    public int Create(Task item)
    {
        // Load existing tasks from XML
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);

        // Generate the next available ID and add the Task to the list
        int nextId = Config.NextTaskId;
        tasksList.Add(item with { Id = nextId });

        // Save the updated list back to XML
        XMLTools.SaveListToXMLSerializer(tasksList, s_task_xml);

        return nextId;
    }

    /// <summary>
    /// Reads a Task from the XML storage based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Task to retrieve.</param>
    /// <returns>The Task with the specified ID, or null if not found.</returns>
    public Task? Read(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        return tasksList.FirstOrDefault(task => task.Id == id);
    }

    /// <summary>
    /// Reads a Task from the XML storage based on a custom filter.
    /// </summary>
    /// <param name="filter">The filter function to apply.</param>
    /// <returns>The first Task that matches the filter, or null if not found.</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);
        return tasksList.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all Tasks from the XML storage, optionally filtered.
    /// </summary>
    /// <param name="filter">An optional filter function to apply.</param>
    /// <returns>A collection of Tasks that match the filter, or all Tasks if no filter is specified.</returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);

        if (filter != null)
            return tasksList.Where(task => filter(task));

        return tasksList.Select(task => task);
    }

    /// <summary>
    /// Updates an existing Task in the XML storage.
    /// </summary>
    /// <param name="item">The updated Task.</param>
    public void Update(Task item)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);

        // Check if a Task with the specified ID exists
        if (!tasksList.Any(task => task.Id == item.Id))
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");

        // Remove the existing Task and add the updated Task
        tasksList.RemoveAll(task => task.Id == item.Id);
        tasksList.Add(item);

        // Save the updated list back to XML
        XMLTools.SaveListToXMLSerializer(tasksList, s_task_xml);
    }

    /// <summary>
    /// Deletes a Task from the XML storage based on its ID.
    /// </summary>
    /// <param name="id">The ID of the Task to delete.</param>
    public void Delete(int id)
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);

        // Check if a Task with the specified ID exists
        if (!tasksList.Any(task => task.Id == id))
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");

        // Change the Task's IsActive property to false
        Task task = tasksList.FirstOrDefault(task => task.Id == id)!;
        tasksList.Remove(task);
        tasksList.Add(task with { IsActive = false });

        // Save the updated list back to XML
        XMLTools.SaveListToXMLSerializer(tasksList, s_task_xml);
    }


    /// <summary>
    /// Deletes all Tasks.
    /// </summary>
    public void DeleteAll()
    {
        List<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>(s_task_xml);

        // Remove all the tasks
        tasksList.Clear();

        // Save the updated list back to XML
        XMLTools.SaveListToXMLSerializer(tasksList, s_task_xml);

        // Reset the next ID to the default value
        XMLTools.ResetId("data-config", "NextTaskId");
    }
}
