namespace PL;

using System;
using System.Collections;

/// <summary>
/// Represents a collection of EngineerExperience values.
/// </summary>
internal class EngineerCollection : IEnumerable
{
    // Collection of EngineerExperience values
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
        (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    /// <summary>
    /// Returns an enumerator that iterates through the EngineerExperience values.
    /// </summary>
    /// <returns>An enumerator for EngineerExperience values.</returns>
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

/// <summary>
/// Represents a collection of Status values.
/// </summary>
internal class TaskCollection : IEnumerable
{
    // Collection of Status values
    static readonly IEnumerable<BO.Status> s_enums =
        (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    /// <summary>
    /// Returns an enumerator that iterates through the Status values.
    /// </summary>
    /// <returns>An enumerator for Status values.</returns>
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

///// <summary>
///// Represents a collection of Status values.
///// </summary>
//internal class TaskCollectionComplexity : IEnumerable
//{
//    // Collection of Status values
//    static readonly IEnumerable<BO.EngineerExperience> s_enums =
//        (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

//    /// <summary>
//    /// Returns an enumerator that iterates through the Status values.
//    /// </summary>
//    /// <returns>An enumerator for Status values.</returns>
//    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
//}
