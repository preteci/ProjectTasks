## ProjectTasks 

ProjectTasks is an .NET ASP core REST Api written in Layered Arhitecture using C#

To run this project download this repository and configure your ConnectionString inside **appsettings.json**

Nuget Packeges required for this project:

- Microsoft.EntitiyFrameworkCore
- Microsoft.EntitiyFrameworkCore.Desing
- Microsoft.EntitiyFrameworkCore.SqlServer

### List of endpoints

| Endpoint  | REST Method | Description |
| --- | --- | --- |
| /api/projects  | GET | Returns all projects from collection |
| /api/projects/:id  | GET | Returns specific project from collection |
| /api/projects | POST  | Adds new project to collection |
| /api/projects/:id  | DELETE | Delete specific project from collection |
| /api/projects/:id  | PUT | Updates specific project from collection |
| /api/projects/:projectId/tasks/:taskId  | POST | Add specific task to specific project |
| /api/projects/:projectId/tasks/:taskId  | DELETE | Remove specific task from project |
| /api/tasks  | GET | Returns all tasks from collection |
| /api/tasks  | POST | Adds tasks to collection |
| /api/tasks/:id  | GET | Returns specific task from collection |
| /api/tasks/:id  | DELETE | Removes specific task from collection |
| /api/tasks/:id  | PUT | Updates specific task from collection |




