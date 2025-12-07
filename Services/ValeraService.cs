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
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // подпись по схеме HS256 с помощью ключа

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token); 
        }

    //     public Task<TokenResponse> GenerateTokenWithExpiry(IdentityUser user)
    //     {
    //     var claims = new List<Claim>
    //     {
    //         new Claim(ClaimTypes.NameIdentifier, user.Id), // UserId (из Identity)
    //         new Claim(JwtRegisteredClaimNames.Sub, user.Id),   
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
    //         new Claim(ClaimTypes.Email, user.Email),           // Email
    //         new Claim(ClaimTypes.Name, user.UserName)          // UserName (если нужен)
    //     };

    //     // 2. Получаем секретный ключ
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"])); // Секретный ключ должен быть в appsettings.json

    //     // 3. Создаем учетные данные для подписи (Signing Credentials)
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //
        
    //     // Устанавливаем время жизни токена
    //     var expiryTime = DateTime.UtcNow.AddMinutes(30); 

    //     // 4. Создаем сам токен
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var token = new JwtSecurityToken(
    //         issuer: _cfg["Jwt:Issuer"], // Кто выпустил
    //         audience: _cfg["Jwt:Audience"], // Для кого
    //         claims: claims,
    //         signingCredentials: creds,
    //         expires: expiryTime
    //         // ...
    //     ); 

    //     return new TokenResponse
    //     {
    //         Token = tokenHandler.WriteToken(token),
    //         ExpiryTimeUtc = expiryTime // Возвращаем время истечения
    //     };
    // }
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
    // Метод, который будет вызываться при запуске приложения
    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Список ролей, которые нам нужны
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            // Проверяем, существует ли роль
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                // Если не существует, создаем ее
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    // Обработка ошибок, если не удалось создать роль
                    Console.WriteLine($"Error creating role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
//     public class AuthorizationLoggerHandler : IAuthorizationHandler
// {
//     private readonly ILogger<AuthorizationLoggerHandler> _logger;

//     public AuthorizationLoggerHandler(ILogger<AuthorizationLoggerHandler> logger)
//     {
//         _logger = logger;
//     }

//     public Task HandleAsync(AuthorizationHandlerContext context)
//     {
//         // Проверяем, что проблема в невыполненных требованиях
//         if (!context.HasSucceeded)
//         {
//             var user = context.User;
            
//             var userName = user?.Identity?.Name ?? "Anonymous";
            
//             _logger.LogWarning("!!! AUTHORIZATION FAILED !!!");
//             _logger.LogWarning("User: {UserName} is not authorized for resource.", userName);
            
//             // Логируем все Claims (включая РОЛИ)
//             _logger.LogWarning("User Claims:");
//             foreach (var claim in user.Claims)
//             {
//                 _logger.LogWarning("- Type: {Type} | Value: {Value}", claim.Type, claim.Value);
//             }

//             // Логируем, какие требования не были выполнены
//             _logger.LogWarning("Failed Requirements:");
//             foreach (var requirement in context.PendingRequirements)
//             {
//                 _logger.LogWarning("- Requirement Type: {RequirementType}", requirement.GetType().Name);
                
//                 // Если это требование роли, логируем, какую роль ждали
//                 if (requirement is RolesAuthorizationRequirement roleRequirement)
//                 {
//                     _logger.LogWarning("  Expected Roles: {Roles}", string.Join(", ", roleRequirement.AllowedRoles));
//                 }
                
//                 // Если это требование аутентификации
//                 if (requirement is DenyAnonymousAuthorizationRequirement)
//                 {
//                     _logger.LogWarning("  Requires authenticated user (Deny Anonymous).");
//                 }
//             }
//         }
//         return Task.CompletedTask;
//     }
// }
}
