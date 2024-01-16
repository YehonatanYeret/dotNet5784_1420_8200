using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class DalDoesNotExistException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalDoesNotExistException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DalDoesNotExistException(string? message) : base(message) { }
    }

    [Serializable]
    public class DalAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalAlreadyExistsException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DalAlreadyExistsException(string? message) : base(message) { }
    }

    [Serializable]
    public class DalDeletionImpossible : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DalDeletionImpossible"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DalDeletionImpossible(string? message) : base(message) { }
    }

    [Serializable]
   public class DalXMLFileLoadCreateException : Exception
    {
        public DalXMLFileLoadCreateException(string? message) : base(message) { }

        public DalXMLFileLoadCreateException(string? message, Exception? innerException) : base(message, innerException) { }

        protected DalXMLFileLoadCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
