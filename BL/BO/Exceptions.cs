namespace BO;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Internal class for handling exceptions within the BO namespace.
/// </summary>

/// <summary>
/// Custom exception class for the scenario where a business logic entity does not exist.
/// </summary>
[Serializable]
public class BLDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BLDoesNotExistException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BLDoesNotExistException(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the BLDoesNotExistException class with an additional inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BLDoesNotExistException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of BLDoesNotExistException.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
    /// <param name="context">The StreamingContext that represents the source or destination of the serialized stream.</param>
    protected BLDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

/// <summary>
/// Custom exception class for the scenario where a business logic entity already exists.
/// </summary>
[Serializable]
public class BLAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BLAlreadyExistsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BLAlreadyExistsException(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the BLAlreadyExistsException class with an additional inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BLAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of BLAlreadyExistsException.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
    /// <param name="context">The StreamingContext that represents the source or destination of the serialized stream.</param>
    public BLAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

/// <summary>
/// Custom exception class for the scenario where deletion of a business logic entity is impossible.
/// </summary>
[Serializable]
public class BLDeletionImpossible : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BLDeletionImpossible"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BLDeletionImpossible(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the BLDeletionImpossible class with an additional inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BLDeletionImpossible(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of BLDeletionImpossible.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
    /// <param name="context">The StreamingContext that represents the source or destination of the serialized stream.</param>
    public BLDeletionImpossible(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
/// <summary>
/// Custom exception class for the scenario where a business logic entity is not correct.
/// </summary>
[Serializable]
public class BLValueIsNotCorrectException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BLValueIsNotCorrectException"/> class with a specified error message.
    /// </summary>
    /// <param name="message"></param>
    public BLValueIsNotCorrectException(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the BLValueIsNotCorrectException class with an additional inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public BLValueIsNotCorrectException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of BLValueIsNotCorrectException.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected BLValueIsNotCorrectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
