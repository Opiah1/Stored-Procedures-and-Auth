using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using SPandAuth.Data;
using SPandAuth.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SPandAuth.AuthClass
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        public JWTManagerRepository(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }
        public Tokens Authenticate(UserLogin user)
        {
            User dbuser = _context.Users.FirstOrDefault(x => x.Username == user.UserName);
            if (dbuser == null || !VerifyPasswordHash(user.Password, dbuser.PasswordHash, dbuser.PasswordSalt)) 
            {
                return null;
            }
            dbuser.LastLoginTime = DateTime.Now;
            _context.SaveChanges();

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            List<Claim> userclaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dbuser.Username),
                new Claim(ClaimTypes.Email, dbuser.Email)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );
            return new Tokens { Token = tokenHandler.WriteToken(token) };
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
