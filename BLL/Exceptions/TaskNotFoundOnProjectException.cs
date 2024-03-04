using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    [Serializable]
    public class TaskNotFoundOnProjectException : Exception
    {
        public TaskNotFoundOnProjectException(string message) : base(message) { }
    }
}
