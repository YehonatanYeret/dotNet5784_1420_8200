using System.Diagnostics.Contracts;

namespace BO;

/// <summary>
/// The type of the engineer.
/// </summary>
public enum EngineerExperience { Beginner, AdvancedBeginner, Intermediate, Advanced, Expert }

/// <summary>
/// The status of the task.
/// </summary>
public enum Status { None, Unscheduled, Scheduled, OnTrack, Done }

public enum ProjectStatus { NotStarted, InProgress, Done }