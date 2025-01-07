using TelgeProject.Entity;
using TelgeProject.Models;

namespace TelgeProject.Interface
{
    public interface IDepartmentRepository
    {
        Task<(IEnumerable<DeparmentViewModel> deparments, int TotalCount)> GetAllDeparmentsAsync(string searchTerm, int pageNumber, int pageSize, int userId);

        Task AddDeparmentAsync(DeparmentViewModel deparment);
        Task<DeparmentViewModel> GetDepartmentByIdAsync(int id);
        Task UpdateDepartmentAsync(DeparmentViewModel department);
        Task DeleteDepartmentAsync(int id);
    }
}
