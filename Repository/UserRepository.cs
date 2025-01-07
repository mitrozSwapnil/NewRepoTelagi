using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Models;

namespace TelgeProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly db_telgeprojectContext _context;

        public UserRepository(db_telgeprojectContext context)
        {
            _context = context;
        }
        //public async Task<(IEnumerable<UserViewModel> users, int TotalCount)> GetAllUsersAsync(string searchTerm, int pageNumber, int pageSize)
        //{

        //   var userlist=_context.TblUsers.Where(x=>x.IsDeleted==false).ToList();
        //    bool isStatusSearch = searchTerm.ToLower().Contains("in");

        //    var query = from user in _context.TblUsers
        //                join userRole in _context.TblUsersroles on user.UserId equals userRole.RoleId
        //                where user.IsDeleted == false &&
        //                      (string.IsNullOrEmpty(searchTerm) ||
        //                       user.FullName.Contains(searchTerm) ||
        //                       user.Email.Contains(searchTerm) ||
        //                       user.Contact.Contains(searchTerm) ||
        //                       (isStatusSearch && user.Status == 1) || // Filter by integer Status
        //                       user.Responsibilities.Contains(searchTerm) ||
        //                       userRole.Role.Contains(searchTerm)) // Filter by Role name
        //                                                           //user.Role.Contains(searchTerm))
        //                select new UserViewModel
        //                {

        //                    UserId = user.UserId,
        //                    Contact = user.Contact,
        //                    Email = user.Email,
        //                    FullName = user.FullName,
        //                    staus = user.Status == 0 ? "Active" : "InActive", // Format for display only
        //                    Profile = user.Profile,
        //                    Responsibilities = user.Responsibilities,
        //                    Role = userRole.Role // Assuming RoleName is the field in the UserRole table
        //                };

        //    int totalCount = await query.CountAsync();
        //    var userslist = await query
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    return (userslist, totalCount);
        //}

        public async Task<(IEnumerable<UserViewModel> users, int TotalCount)> GetAllUsersAsync(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                var query = from user in _context.TblUsers
                            where user.IsDeleted == false
                            join role in _context.TblUsersroles
                            on user.Role equals role.RoleId into roles
                            from role in roles.DefaultIfEmpty() 
                            select new UserViewModel
                            {
                                UserId = user.UserId,
                                Contact = user.Contact,
                                Email = user.Email,
                                FullName = user.FullName,
                                staus = user.Status == 0 ? "Active" : "Inactive",
                                Profile = user.Profile,
                                Responsibilities = user.Responsibilities,
                                Role = role != null ? role.Role : null 
                            };

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(o => o.FullName.Contains(searchTerm) ||
                                             o.staus.Contains(searchTerm) ||
                                             o.Role.Contains(searchTerm) ||
                                             o.Email.Contains(searchTerm));
                }

                int totalCount = await query.CountAsync();

                var usersList = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (usersList, totalCount);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");

                return (Enumerable.Empty<UserViewModel>(), 0);
            }
        }



        public async Task<IEnumerable<TblUser>> GetAllUsersAsync()
        {
            return await _context.TblUsers.Where(u => u.IsDeleted == false).ToListAsync();
        }

        public async Task<TblUser> GetUserByIdAsync(int id)
        {
            return await _context.TblUsers.FirstOrDefaultAsync(u => u.UserId == id && u.IsDeleted==false);
        }

      

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.TblUsers.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }

}
