using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    [Serializable]
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string message) : base(message) { }
    }
}