using System.Runtime.Serialization;

namespace DO;

[Serializable]
public class DalNotExistException : Exception
{
     public DalNotExistException(string? message) : base(message)
    {
    }

    public DalNotExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DalNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

[Serializable]
public class DalAllreadyExistException : Exception
{
     public DalAllreadyExistException(string? message) : base(message)
    {
    }

    public DalAllreadyExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DalAllreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}