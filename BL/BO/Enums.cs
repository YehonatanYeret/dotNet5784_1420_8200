using System.Diagnostics.Contracts;

namespace BO;

/// <summary>
/// The type of the engineer.
/// </summary>
public enum EngineerExperience { Beginner, AdvancedBeginner, Intermediate, Advanced, Expert }

public enum Status { Unscheduled, Scheduled, OnTrack, InJeopardy, Done }