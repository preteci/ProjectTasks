using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IFileService
    {
        public Task UploadFileToTaskAsync(int taskId, Stream fileStream, string fileName);
        public Task<IEnumerable<string>> GetListOfFilesFromTaskAsync(int taskId);

        public Task<Stream> DownloadFileFromTaskAsync(int taskId, string fileName);

        public Task UploadFileProjectAsync(int projectId, Stream fileStream, string fileName);
        public Task<IEnumerable<string>> GetListOfFilesFromProjectAsync(int projectId);
        public Task<Stream> DownloadFileFromProjectAsync(int projectId, string fileName);
    }
}
