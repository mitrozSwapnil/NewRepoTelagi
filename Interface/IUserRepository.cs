using TelgeProject.Entity;
using TelgeProject.Models;

namespace TelgeProject.Interface
{
    public interface IUserRepository
    {
        Task<(IEnumerable<UserViewModel> users, int TotalCount)> GetAllUsersAsync(string searchTerm,int pageNumber, int pageSize);
        Task<IEnumerable<TblUser>> GetAllUsersAsync();
        Task<TblUser> GetUserByIdAsync(int id);
      
      
        Task DeleteUserAsync(int id);
    }
}
