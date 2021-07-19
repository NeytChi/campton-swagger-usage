using Campton.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campton.Commands
{
    /// <summary>
    /// Модель ответа после логина
    /// </summary>
    public class LoginDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public long LastLoginAt { get; set; }
        public long CreatedAt { get; set; }
        public string Hash { get; set; }

        public LoginDto(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.Name;
            LastLoginAt = user.LastLoginAt;
            CreatedAt = user.CreatedAt;
            Hash = user.Hash;
        }
    }
}
