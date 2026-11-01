using web.Models;
using web.Service;
using web.Data;
using web.ValeraController;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

// по сути это инварианты для передачи данных
namespace web.DTOs
{
    public class ValeraDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "Valera";
        public bool? is_alive { get; set; } = true;
        public int? HP { get; set; }
        public int? MP { get; set; }
        public int? FT { get; set; }
        public int? CF { get; set; }
        public int? MN { get; set; }
    }

    public class RegisterDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Username { get; set; } = default!;
    }
    public class LoginDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}