using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    [Serializable]
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string message) : base(message) { }
    }
}