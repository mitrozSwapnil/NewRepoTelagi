using TelgeProject.Entity;
using TelgeProject.Models;

namespace TelgeProject.Interface
{
    public interface ICustomerRepository
    {
        Task<(IEnumerable<CustomerViewModel> customers, int TotalCount)> GetAllCustomersAsync(string searchTerm, int pageNumber, int pageSize, int userId);
        Task<IEnumerable<TblCustomer>> GetAllCustomersAsync();

        Task AddCustomerAsync(CustomerViewModel customer);
        Task<CustomerViewModel> GetCustomerByIdAsync(int id);

        Task UpdateCustomerAsync(CustomerViewModel customer);
        Task DeleteCustomerAsync(int id);
    }
}
