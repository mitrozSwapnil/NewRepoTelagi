﻿using TelgeProject.Entity;

namespace TelgeProject.Interface
{
    public interface IAuthRepository
    {
        TblUser GetUserByUsername(string username);
        void SaveToken(TblUsertoken token);
        Task<string> GetUserRoleAsync(string userId);
    }
}