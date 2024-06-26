﻿namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineer_xml = "engineers";
    readonly string s_user_xml = "users";

    /// <summary>
    /// Creates a new Engineer.
    /// </summary>
    /// <param name="item">The Engineer object to be created.</param>
    /// <returns>The ID of the newly created Engineer.</returns>
    public int Create(Engineer item)
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineer_xml);

        if (engineersList.Any(engineer => engineer.Id == item.Id))
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");

        engineersList.Add(item);
        XMLTools.SaveListToXMLSerializer(engineersList, s_engineer_xml);

        return item.Id;
    }

    /// <summary>
    /// Reads an Engineer by ID.
    /// </summary>
    /// <param name="id">The ID of the Engineer to read.</param>
    /// <returns>The Engineer object if found, otherwise null.</returns>
    public Engineer? Read(int id)
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineer_xml);
        Engineer? engineer = engineersList.FirstOrDefault(engineer => engineer.Id == id);
        if (engineer != null)
            return engineer;
        return null;
    }

    /// <summary>
    /// Reads an Engineer based on a custom filter.
    /// </summary>
    /// <param name="filter">The filter function to apply.</param>
    /// <returns>The first Engineer that satisfies the filter, otherwise null.</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineer_xml);

        Engineer? engineer = engineersList.FirstOrDefault(filter);
        if (engineer != null)
            return engineer;
        return null;
    }

    /// <summary>
    /// Reads all Engineers with an optional filter.
    /// </summary>
    /// <param name="filter">Optional filter function to apply.</param>
    /// <returns>A collection of Engineers that satisfy the filter, or all Engineers if no filter is provided.</returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineer_xml);

        if (filter != null)
            return engineersList.Where(filter);

        return engineersList.Select(engineer => engineer);
    }

    /// <summary>
    /// Updates an existing Engineer.
    /// </summary>
    /// <param name="item">The updated Engineer object.</param>
    public void Update(Engineer item)
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineer_xml);

        if (!engineersList.Any(engineer => engineer.Id == item.Id))
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");

        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        string pass="";

        // remove the old engineer from the list and the user from the list
        foreach (DO.Engineer engineer in engineersList)
        {
            if (engineer.Id == item.Id)
            {
                engineersList.Remove(engineer);
                pass = usersList.First(user => user.Email == engineer.Email).Password;
                usersList.Remove(usersList.First(user => user.Email == engineer.Email));
                break;
            }
        }

        // add the updated engineer to the list and the user to the likst
        engineersList.Add(item);

        usersList.Add(new DO.User
        {
            Type = DO.UserType.engineer,
            Name = item.Name,
            Password = pass,
            Email = item.Email
        });

        XMLTools.SaveListToXMLSerializer(engineersList, s_engineer_xml);
        XMLTools.SaveListToXMLSerializer(usersList, s_user_xml);
    }

    /// <summary>
    /// Deletes an Engineer by ID.
    /// </summary>
    /// <param name="id">The ID of the Engineer to delete.</param>
    public void Delete(int id)
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineer_xml);

        if (!engineersList.Any(engineer => engineer.Id == id))
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");

        engineersList.RemoveAll(engineer => engineer.Id == id);

        // remove in tasks the engineer with the given id
        foreach (var task in new TaskImplementation().ReadAll(t => t.EngineerId == id))
            new TaskImplementation().Update(task with { EngineerId = null });

        XMLTools.SaveListToXMLSerializer(engineersList, s_engineer_xml);
    }

    /// <summary>
    /// Deletes all Engineers.
    /// </summary>
    public void DeleteAll()
    {
        List<DO.Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<DO.Engineer>(s_engineer_xml);

        // remove all engineers
        engineersList.Clear();

        // save the updated list back to XML
        XMLTools.SaveListToXMLSerializer(engineersList, s_engineer_xml);
    }

}
