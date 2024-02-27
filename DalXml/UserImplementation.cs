namespace Dal;

using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

internal class UserImplementation : IUser
{
    readonly string s_user_xml = "users";

    /// <summary>
    /// Creates a new User.
    /// </summary>
    /// <param name="item">The User object to be created.</param>
    /// <returns>The Email of the newly created User.</returns>
    public string Create(User item)
    {
        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        if (usersList.Any(user => user.Email == item.Email))
            throw new DalAlreadyExistsException($"User with Email={item.Email} already exists");

        usersList.Add(item);
        XMLTools.SaveListToXMLSerializer(usersList, s_user_xml);

        return item.Email;
    }

    /// <summary>
    /// Reads an User by Email.
    /// </summary>
    /// <param name="email">The Email of the User to read.</param>
    /// <returns>The User object if found, otherwise null.</returns>
    public User? Read(string email)
    {
        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        return usersList.FirstOrDefault(user => user.Email == email);
    }

    /// <summary>
    /// Reads an User based on a custom filter.
    /// </summary>
    /// <param name="filter">The filter function to apply.</param>
    /// <returns>The first User that satisfies the filter, otherwise null.</returns>
    public User? Read(Func<User, bool> filter)
    {
        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        return usersList.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all User with an optional filter.
    /// </summary>
    /// <param name="filter">Optional filter function to apply.</param>
    /// <returns>A collection of Users that satisfy the filter, or all Users if no filter is provided.</returns>
    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        if (filter != null)
            return usersList.Where(filter);

        return usersList.Select(user => user);
    }

    /// <summary>
    /// Updates an existing User.
    /// </summary>
    /// <param name="item">The updated User object.</param>
    public void Update(User item)
    {
        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        if (!usersList.Any(user => user.Email == item.Email))
            throw new DalDoesNotExistException($"User with Email={item.Email} does not exist");

        usersList.RemoveAll(user => user.Email == item.Email);
        usersList.Add(item);

        XMLTools.SaveListToXMLSerializer(usersList, s_user_xml);
    }

    /// <summary>
    /// Deletes an User by Email.
    /// </summary>
    /// <param name="id">The ID of the User to delete.</param>
    public void Delete(string email)
    {
        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        if (!usersList.Any(user => user.Email == email))
            throw new DalDoesNotExistException($"User with Email={email} does not exist");

        usersList.RemoveAll(user => user.Email == email);

        XMLTools.SaveListToXMLSerializer(usersList, s_user_xml);
    }

    /// <summary>
    /// Deletes all Users and initializes the list with the default admin user.
    /// </summary>
    public void DeleteAll()
    {
        List<DO.User> usersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_user_xml);

        // remove all users
        usersList.Clear();

        usersList.Add(new DO.User
        {
            Type = UserType.manager,
            Name = "admin",
            Password = "Admin123",
            Email = "admin@gmail.com"
        });

        // save the updated list back to XML
        XMLTools.SaveListToXMLSerializer(usersList, s_user_xml);
    }
}
