using web.Models;
using web.Service;
using web.ValeraController;
using Microsoft.EntityFrameworkCore;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<web.Data.AppDbContext>(options =>
            options.UseInMemoryDatabase("ValeraDb"));
        builder.Services.AddScoped<ValeraService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}