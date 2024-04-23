using Azure.Storage.Files.Shares;
using BLL.Exceptions;
using DAL.Repository;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using Microsoft.Extensions.Configuration;

namespace BLL
{
    public class FileService : IFileService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly CloudStorageAccount _storageAccount;
        private readonly ShareServiceClient _serviceClient;
        private CloudFileClient _fileClient;

        public FileService(IProjectRepository projectRepository, ITaskRepository taskRepository, IConfiguration config)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _storageAccount = CloudStorageAccount.Parse(config["AzureFiles:Key"]);
            _fileClient = _storageAccount.CreateCloudFileClient();
            _serviceClient = new ShareServiceClient(config["AzureFiles:Key"]);
        }

        public async Task UploadFileToTaskAsync(int taskId, Stream fileStream, string fileName)
        {
            // grabing a task
            var task = await _taskRepository.GetByIdAsync(taskId);

            // if task is not aviablie we will throw a new execption
            if (task == null)
            {
                throw new TaskNotFoundException("Task not found");
            }

            CloudFileShare fileShare = _fileClient.GetShareReference("projecttasks");

            CloudFileDirectory taskDirectory = fileShare.GetRootDirectoryReference().GetDirectoryReference("Task").GetDirectoryReference("" + taskId);
            await taskDirectory.CreateIfNotExistsAsync();

            CloudFile cloudFile = taskDirectory.GetFileReference(fileName);
            await cloudFile.UploadFromStreamAsync(fileStream);
        }


        public async Task<IEnumerable<string>> GetListOfFilesFromTaskAsync(int taskId)
        {

            // check if task exist
            if (await _taskRepository.GetByIdAsync(taskId) == null)
                throw new TaskNotFoundException("Task not found");

            // establish client
            ShareDirectoryClient directoryClient = 
                _serviceClient.GetShareClient("projecttasks")
                .GetRootDirectoryClient()
                .GetSubdirectoryClient("Task")
                .GetSubdirectoryClient("" + taskId);

            var listOfFiles = new List<string>();

            // get back all list of files
            await foreach (var item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                listOfFiles.Add(item.Name);
            }

            return listOfFiles;
        }

        
        public async Task<Stream> DownloadFileFromTaskAsync(int taskId, string fileName)
        {
            // check if task exist
            if (await _taskRepository.GetByIdAsync(taskId) == null)
                throw new TaskNotFoundException("Task not found");

            // making a client
            ShareFileClient fileClient = 
                _serviceClient.GetShareClient("projecttasks")
                .GetRootDirectoryClient()
                .GetSubdirectoryClient("Task")
                .GetSubdirectoryClient("" + taskId)
                .GetFileClient(fileName);

            // checking if file exsits on azure files
            if (!await fileClient.ExistsAsync())
                throw new FileNotFoundException("File was not found");

            return await fileClient.OpenReadAsync();
        }

        public async Task UploadFileProjectAsync(int projectId, Stream fileStream, string fileName)
        {
            // grab project
            if (await _projectRepository.GetByIdAsync(projectId) == null)
                throw new ProjectNotFoundException("Project not found");

            // create client
            CloudFileDirectory projectDirectory = 
                _fileClient.GetShareReference("projecttasks")
                .GetRootDirectoryReference()
                .GetDirectoryReference("Project")
                .GetDirectoryReference("" + projectId);

            // create directory if dosnt exist
            await projectDirectory.CreateIfNotExistsAsync();

            // upload file
            CloudFile cloudFile = projectDirectory.GetFileReference(fileName);
            await cloudFile.UploadFromStreamAsync(fileStream);
        }

        public async Task<IEnumerable<string>> GetListOfFilesFromProjectAsync(int projectId)
        { 
            // check if project exist
            if (await _projectRepository.GetByIdAsync(projectId) == null)
                throw new ProjectNotFoundException("Project not found");

            // establish client
            ShareDirectoryClient directoryClient = 
                _serviceClient.GetShareClient("projecttasks")
                .GetRootDirectoryClient()
                .GetSubdirectoryClient("Project")
                .GetSubdirectoryClient("" + projectId);

            var listOfFiles = new List<string>();

            // get back all list of files
            await foreach (var item in directoryClient.GetFilesAndDirectoriesAsync())
            {
                listOfFiles.Add(item.Name);
            }

            return listOfFiles;
        }

        public async Task<Stream> DownloadFileFromProjectAsync(int projectId, string fileName)
        {
            if (await _projectRepository.GetByIdAsync(projectId) == null)
                throw new ProjectNotFoundException(" Project not found");

            // making a client
            ShareFileClient fileClient =
                _serviceClient.GetShareClient("projecttasks")
                .GetRootDirectoryClient()
                .GetSubdirectoryClient("Project")
                .GetSubdirectoryClient("" + projectId)
                .GetFileClient(fileName);

            // checking if file exsits on azure files
            if (!await fileClient.ExistsAsync())
                throw new FileNotFoundException("File was not found");
            
            // return file
            return await fileClient.OpenReadAsync();
        }

    }
}
