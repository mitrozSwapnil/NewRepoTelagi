using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly db_telgeprojectContext _context;

        public CustomerRepository(db_telgeprojectContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<CustomerViewModel> customers, int TotalCount)> GetAllCustomersAsync(string searchTerm, int pageNumber, int pageSize, int userId)
        {
            try
            {
                var customerQuery = from customer in _context.TblCustomers
                                    where customer.IsDeleted == false && customer.CreatedBy == userId
                                    select new CustomerViewModel
                                    {
                                        CustomerId = customer.Id,
                                        FullName = customer.FullName,
                                        BussinessName = customer.BussinessName,
                                        Email = customer.Email,
                                        Contact = customer.Contact,
                                        Address = customer.Address,
                                        Gstno = customer.Gstno,
                                        CreatedAt = customer.CreatedAt,
                                        CreatedBy = customer.CreatedBy

                                    };

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    customerQuery = customerQuery.Where(o => o.FullName.Contains(searchTerm) ||
                                             o.Email.Contains(searchTerm) ||
                                             o.Address.Contains(searchTerm) ||
                                             o.Contact.Contains(searchTerm) ||
                                             o.Gstno.Contains(searchTerm) ||
                                             o.BussinessName.Contains(searchTerm));
                }

                int totalCount = await customerQuery.CountAsync();

                var customesList = await customerQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (customesList, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (Enumerable.Empty<CustomerViewModel>(), 0);
            }


        }

        public async Task<IEnumerable<TblCustomer>> GetAllCustomersAsync()
        {
            return await _context.TblCustomers.Where(u => u.IsDeleted == false).ToListAsync();
        }

        public async Task AddCustomerAsync(CustomerViewModel customer)
        {

            var addCustomers = new TblCustomer
            {
                FullName = customer.FullName,
                Email = customer.Email,
                Address = customer.Address,
                Contact = customer.Contact,
                Gstno = customer.Gstno,
                BussinessName = customer.BussinessName,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = customer.CreatedBy
            };
            _context.TblCustomers.Add(addCustomers);
            await _context.SaveChangesAsync();
        }



        public async Task<CustomerViewModel> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.TblCustomers
            .FirstOrDefaultAsync(c => c.Id == id);

            if (customer != null)
            {
                return new CustomerViewModel
                {
                    CustomerId = customer.Id,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    Address = customer.Address,
                    Contact = customer.Contact,
                    Gstno = customer.Gstno,
                    BussinessName = customer.BussinessName
                };
            }
            return null;


        }

        public async Task UpdateCustomerAsync(CustomerViewModel customer)
        {
            var existingCustomer = await _context.TblCustomers
            .FirstOrDefaultAsync(c => c.Id == customer.CustomerId);

            if (existingCustomer == null)
            {
                throw new Exception("Customer not found");
            }

            existingCustomer.FullName = customer.FullName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Address = customer.Address;
            existingCustomer.Contact = customer.Contact;
            existingCustomer.Gstno = customer.Gstno;
            existingCustomer.BussinessName = customer.BussinessName;
            existingCustomer.UpdatedAt = DateTime.Now;
            existingCustomer.UpdatedBy = customer.UpdatedBy;

            await _context.SaveChangesAsync();


        }


        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.TblCustomers.FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            customer.IsDeleted = true;
            _context.TblCustomers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
