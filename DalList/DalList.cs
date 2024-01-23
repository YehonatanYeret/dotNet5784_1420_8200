﻿namespace Dal;
using DalApi;

/// <summary>
/// Represents a Data Access Layer (DAL) list providing access to different entities.
/// </summary>
sealed internal class DalList : IDal
{
    private DalList() { }

    public static IDal Instance { get; } = new DalList();
    //public static Lazy<IDal> Instance { get; } = new Lazy<IDal>(() => new DalList());

    /// <summary>
    /// Gets an instance of the <see cref="IEngineer"/> interface for accessing engineer-related data.
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Gets an instance of the <see cref="ITask"/> interface for accessing task-related data.
    /// </summary>
    public ITask Task => new TaskImplementation();

    /// <summary>
    /// Gets an instance of the <see cref="IDependency"/> interface for accessing dependency-related data.
    /// </summary>
    public IDependency Dependency => new DependencyImplementation();
}

