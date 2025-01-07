using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly db_telgeprojectContext _context;

        public DepartmentRepository(db_telgeprojectContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<DeparmentViewModel> deparments, int TotalCount)> GetAllDeparmentsAsync(string searchTerm, int pageNumber, int pageSize, int userId)
        {
            try
            {
                var departmentQuery = from deparment in _context.TblDepartments
                                      where deparment.IsDeleted == false && deparment.CreatedBy == userId
                                      select new DeparmentViewModel
                                      {
                                          DepartmentId = deparment.DepartmentId,
                                          DepartmentName = deparment.DepartmentName
                                      };

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    departmentQuery = departmentQuery.Where(o => o.DepartmentName.Contains(searchTerm));

                }

                int totalCount = await departmentQuery.CountAsync();

                var customesList = await departmentQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (customesList, totalCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (Enumerable.Empty<DeparmentViewModel>(), 0);
            }
        }

        public async Task AddDeparmentAsync(DeparmentViewModel deparment)
        {
            var addDepartment = new TblDepartment
            {
                DepartmentName = deparment.DepartmentName,
                IsDeleted = false,
                CreatedBy = deparment.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _context.TblDepartments.Add(addDepartment);
            await _context.SaveChangesAsync();
        }

        public async Task<DeparmentViewModel> GetDepartmentByIdAsync(int id)
        {
            return await _context.TblDepartments
            .Where(d => d.DepartmentId == id && d.IsDeleted == false)
            .Select(d => new DeparmentViewModel
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            })
            .FirstOrDefaultAsync();
        }

        public async Task UpdateDepartmentAsync(DeparmentViewModel department)
        {
            var existingDepartment = await _context.TblDepartments
            .FirstOrDefaultAsync(d => d.DepartmentId == department.DepartmentId);

            if (existingDepartment == null)
            {
                throw new Exception("Department not found");
            }

            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.UpdatedAt = DateTime.Now;
            existingDepartment.UpdatedBy = department.UpdatedBy;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _context.TblDepartments.FirstOrDefaultAsync(x => x.DepartmentId == id);

            if(department == null)
            {
                throw new Exception("Department not found");
            }
            department.IsDeleted = true;
            _context.TblDepartments.Update(department);
            await _context.SaveChangesAsync();
        }
    }
}
