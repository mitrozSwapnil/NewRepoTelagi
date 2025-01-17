﻿
using Microsoft.EntityFrameworkCore;
using TelgeProject.Entity;
using TelgeProject.Interface;

namespace TelgeProject.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly db_telgeprojectContext _context;
        public AuthRepository(db_telgeprojectContext context)
        {
            _context = context;
        }

        public TblUser GetUserByUsername(string username)
        {
            return _context.TblUsers.SingleOrDefault(u => u.Contact == username);
        }

        public async Task<string> GetUserRoleAsync(string userId)
        {
            var userRole = await _context.TblUsersroles
                                      .Where(ur => ur.Role == userId)
                                      .Select(ur => ur.Role)
                                      .FirstOrDefaultAsync();

            return userRole;
        }

        public void SaveToken(TblUsertoken token)
        {
            _context.TblUsertokens.Add(token);
            _context.SaveChanges();
        }


    }
}
