﻿    namespace Dal;
using DalApi;
using DO;

internal class DependencyImplementation : IDependency
{
    //create a new dependency
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;//get the next id
        Dependency dependency = item with { Id = id };//create a new dependency with the new id
        DataSource.Dependencies.Add(dependency);    //add the dependency to the list
        return id;
    }

    //read a dependency and return it. if not found return null
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(dependency => dependency.Id == id);
    }

    //return a copy of the list of dependencies
    public IEnumerable<Dependency> ReadAll()
    {
        IEnumerable<Dependency> dependencies = DataSource.Dependencies;
        return from dependency in dependencies
               select dependency;
    }
    
    //update a dependency by removing the old one and adding the new one
    public void Update(Dependency item)
    {
        Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == item.Id);
        if (dependency == null)
            throw new Exception($"Dependency with ID={item.Id} does not exist");

        DataSource.Dependencies.RemoveAll(temp=> temp.Id == dependency.Id);
        DataSource.Dependencies.Add(item);
    }

    //delete a dependency. always throw an exception
    public void Delete(int id)
    {
        Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == id);
        if (dependency == null)
            throw new Exception($"Dependency with ID={id} does not exist");
        DataSource.Dependencies.RemoveAll(temp=> temp.Id == id);
    }
}
