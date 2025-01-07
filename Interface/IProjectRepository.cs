using TelgeProject.Entity;
using TelgeProject.Models;

namespace TelgeProject.Interface
{
    public interface IProjectRepository
    {
        Task<(IEnumerable<ProjectViewModel> projets, int TotalCount)> GetAllProjectsAsync(string searchTerm, int pageNumber, int pageSize, int userId);
        //Task<IEnumerable<TblProject>> GetAllProjectsAsync();

        Task AddProjectAsync(ProjectViewModel project);

        Task<ProjectViewModel> GetProjectByIdAsync(int projectId);

        Task UpdateProjectAsync(ProjectViewModel project);

        //Task SaveDocumentAsync(byte[] fileBytes);

        //Task<(IEnumerable<ProjectViewModel> Projects, int TotalCount)> GetAllProjectsAsync(int pageNumber, int pageSize);
        //Task<ProjectViewModel> GetProjectByIdAsync(int id);
        //Task AddProjectAsync(TblProject project);
        //Task UpdateProjectAsync(TblProject project);
        //Task DeleteProjectAsync(int id);
    }
}
