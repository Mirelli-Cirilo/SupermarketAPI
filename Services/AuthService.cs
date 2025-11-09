using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthService(AppDbContext Context, TokenService tokenService)
        {
            _context = Context;
            _tokenService = tokenService;
        }


        public (bool Success, string Message, string? Token, string? Role)Login(string username,
            string password)
        { 
            var user = _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password); 
            if (user == null) 
            { 
                return (false, "Username or Password are Invalid", null, null);
            }
            var token = _tokenService.GenerateToken(user); 
            return (true, "Login Successful", token, user.Role);
        }
    }
}
