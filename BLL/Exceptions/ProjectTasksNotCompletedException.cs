using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    [Serializable]
    public class ProjectTasksNotCompletedException : Exception
    {
        public ProjectTasksNotCompletedException(string message) : base(message) { }
    }
}