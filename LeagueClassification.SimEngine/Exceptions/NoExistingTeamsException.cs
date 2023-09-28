using System.Runtime.Serialization;

namespace LeagueClassification.SimEngine.Exceptions
{
    [Serializable]
    public class NoExistingTeamsException : Exception
    {
        public NoExistingTeamsException()
        {
        }

        public NoExistingTeamsException(string? message) : base(message)
        {
        }

        public NoExistingTeamsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoExistingTeamsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}