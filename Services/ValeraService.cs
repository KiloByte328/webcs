using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using web.Data;
using web.Models;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure; 

namespace web.Service
{
    public class AuthService : IJwtService
    {
        private readonly IConfiguration _cfg;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthService(IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _cfg = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(IdentityUser user)
        {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id), 
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
            new Claim(ClaimTypes.Email, user.Email!),
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); 

        var keyString = _cfg["Jfw:Key"] ?? "ABCDEFGHIJKLMNOPQRSTYVWXYZTHATISGOINGONBRUHaiougfiuawfuygqwafjkdbawjsfhbkabfkjasvbfjklasbkbsajkbfawjlkbfkaswfasfjhawvbfbasjkfvb";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }

    public class ValeraService
    {
        private readonly AppDbContext _context;
        public ValeraService(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<IEnumerable<Valera>> GetAllValeras()
        {
            
            return  await _context.Valeras.ToListAsync();
        }
        public async Task<IEnumerable<Valera>> GetValerasByOwner(string Owner)
        {
            return await _context.Valeras.Where(v => v.ValeraOwner == Owner).ToListAsync();
        }
        public async Task<Valera?> GetValeraById(int id)
        {
            return await _context.Valeras.FirstOrDefaultAsync(v => v.Id == id);
        }
        public async Task AddValeraToDb(Valera valera)
        {
            await _context.Valeras.AddAsync(valera);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateValeraInDb(Valera valera)
        {
            _context.Valeras.Update(valera);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteValera(Valera valera)
        {
            _context.Valeras.Remove(valera);
            await _context.SaveChangesAsync();
        }
    }
    public static class RoleInitializer
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole(roleName);
                    var result = await roleManager.CreateAsync(role);

                    if (!result.Succeeded)
                    {
                        Console.WriteLine($"Error creating role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }
}
