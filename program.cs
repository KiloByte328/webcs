using web.Models;
using web.Service;
using web.Data;
using web.ValeraController;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
public class Program
{
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContextPool<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<AppDbContext>();
        builder.Services.AddScoped<ValeraService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Valera API",
                Version = "v1",
                Description = "API для управления Валерой"
            });
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReact",
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        builder.Services.AddScoped<IJwtService, AuthService>(); 
        // builder.Services.AddSingleton<IAuthorizationHandler, AuthorizationLoggerHandler>(); 
        builder.Services.AddAuthentication(options =>
        {
            // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // var logfile = File.Open("/log", FileMode.CreateNew);
                        // var log = "JWT Auth Failed: " + context.Exception.Message;
                        // var bts = Encoding.UTF8.GetBytes(log);
                        // var rosb = new ReadOnlySpan<byte>(bts); 
                        // logfile.Write(rosb);
                        // ЗАПИСЬ ОШИБКИ В КОНСОЛЬ (для дебага)
                        Console.WriteLine("JWT Auth Failed: " + context.Exception.Message);
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.Headers["error"] = "JWT Auth Failed: " + context.Exception.Message;
                        return Task.CompletedTask;
                    },
                    // OnChallenge = context =>
                    // {
                    //     context.HandleResponse();
                    //     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //     context.Response.ContentType = "application/json";
                        
                    //     // Записываем общую ошибку
                    //     var errorMessage = context.ErrorDescription ?? "Invalid or missing token.";
                    //     return context.Response.WriteAsync($"{{ \"error\": \"{errorMessage}\" }}");
                    // }
                };
                var keyString = builder.Configuration["Jwt:Key"] ?? "ABCDEFGHIJKLMNOPQRSTYVWXYZTHATISGOINGONBRUHaiougfiuawfuygqwafjkdbawjsfhbkabfkjasvbfjklasbkbsajkbfawjlkbfkaswfasfjhawvbfbasjkfvb";
                // Настройки валидации токена
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuer = true, // Проверять Issuer (Кто выпустил)
                    ValidateAudience = true, // Проверять Audience (Для кого)
                    ValidateLifetime = true, // Проверять срок действия (Expires)
                    ValidateIssuerSigningKey = true, // Проверять секретный ключ
                    

                    ValidIssuer = builder.Configuration["Jwt:Issuer"], // Берется из appsettings.json
                    ValidAudience = builder.Configuration["Jwt:Audience"], // Берется из appsettings.json
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString)) // Секретный ключ
                    
                };
            });
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>();
        builder.Services.ConfigureApplicationCookie(options =>
        {
            // ConfigureApplicationCookie - это shortcut для AddCookie(IdentityConstants.ApplicationScheme, ...)
            
            // Перехватываем событие OnRedirectToLogin
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                // context.Response.Headers["Location"] = "http://localhost:63628/api/Auth/login";
                context.Response.ContentType = "application/json";
                return Task.CompletedTask;
            };
            
            // Перехватываем событие OnRedirectToAccessDenied (для [Authorize(Roles = ...)])
            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;   
                context.Response.ContentType = "application/json";
                return Task.CompletedTask;
            };
        });
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                // Теперь await здесь разрешен!
                await RoleInitializer.SeedRoles(services); 
            }
            catch (Exception ex)
            {
                // Обработка ошибок, если seed не удался
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database roles.");
            }
        }
        app.UseCors("AllowReact");
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthentication(); 
        app.UseAuthorization(); 
        app.MapControllers();
        app.Run();
    }
}