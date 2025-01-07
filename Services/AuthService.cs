using Microsoft.CodeAnalysis.Scripting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TelgeProject.Entity;
using TelgeProject.Interface;
using TelgeProject.Repository;

namespace TelgeProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public TblUser Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public string GenerateToken(TblUser user)
        {
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var role = _userRepository.GetUserRoleAsync(user.Role.ToString());

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Contact),
               // new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
            //return Guid.NewGuid().ToString(); // Simple token generation (replace with JWT if needed)
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                // Verify password against stored hash
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
            catch (Exception ex)
            {
                // Log exception for debugging
                Console.WriteLine($"Error verifying password: {ex.Message}");
                return false;
            }
        }

    }
}