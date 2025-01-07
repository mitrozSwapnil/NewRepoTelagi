using TelgeProject.Entity;
using TelgeProject.Models;

namespace TelgeProject.Interface
{
    public interface IAddTaskRepository
    {
        Task<(IEnumerable<AddTaskViewModel> tasks, int TotalCount)> GetAllTasksAsync(int projectId, string searchTerm, int pageNumber, int pageSize, int userId);
        Task<IEnumerable<TblTask>> GetAllTasksAsync();
        Task AddTaskAsync(AddTaskViewModel addTask);
        //Task GetAllTasksAsync(object searchTerm, object pageNumber, object pageSize);

        Task<AddTaskViewModel> GetTaskByIdAsync(int id);

        Task UpdateTaskAsync(AddTaskViewModel task);

        Task<(IEnumerable<SubTaskViewModel> subTasks, int TotalCount)> GetAllSubTasksAsync(int taskId, string searchTerm, int pageNumber, int pageSize, int userId);

        Task<SubTaskViewModel> GetSubTaskByIdAsync(int id);

        Task UpdateSubTaskAsync(SubTaskViewModel subTask);
    }
}
