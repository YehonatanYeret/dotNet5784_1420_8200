namespace DO;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Custom exception class for the scenario where a data access layer entity does not exist.
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalDoesNotExistException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DalDoesNotExistException(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the DalDoesNotExistException class with an additional inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DalDoesNotExistException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of DalDoesNotExistException.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
    /// <param name="context">The StreamingContext that represents the source or destination of the serialized stream.</param>
    public DalDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

/// <summary>
/// Custom exception class for the scenario where a data access layer entity already exists.
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalAlreadyExistsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DalAlreadyExistsException(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the DalAlreadyExistsException class with an additional inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DalAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of DalAlreadyExistsException.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
    /// <param name="context">The StreamingContext that represents the source or destination of the serialized stream.</param>
    public DalAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

/// <summary>
/// Custom exception class for the scenario where deletion of a data access layer entity is impossible.
/// </summary>
[Serializable]
public class DalDeletionImpossible : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalDeletionImpossible"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DalDeletionImpossible(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the DalDeletionImpossible class with an additional inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DalDeletionImpossible(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of DalDeletionImpossible.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
    /// <param name="context">The StreamingContext that represents the source or destination of the serialized stream.</param>
    public DalDeletionImpossible(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

/// <summary>
/// Custom exception class for handling errors related to XML file loading or creation in data access layer.
/// </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DalXMLFileLoadCreateException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DalXMLFileLoadCreateException(string? message) : base(message) { }

    /// <summary>
    /// Constructor for the DalXMLFileLoadCreateException class with an additional inner exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DalXMLFileLoadCreateException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructor for deserialization of DalXMLFileLoadCreateException.
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data.</param>
    /// <param name="context">The StreamingContext that represents the source or destination of the serialized stream.</param>
    protected DalXMLFileLoadCreateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
